using AgroSolutions.Property.Domain.Events;
using AgroSolutions.Property.Domain.Messaging;
using AgroSolutions.Property.Infrastructure.Extensions;
using AgroSolutions.Property.Infrastructure.Messaging;
using AgroSolutions.Property.Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Serilog;
using Serilog.Context;
using System.Text;
using System.Text.Json;

namespace AgroSolutions.Property.Infrastructure.Subscribers;

public class DeletedUserSubscriber(IServiceProvider serviceProvider, IOptions<RabbitMqOptions> options) : BackgroundService
{
    private readonly string _queue = options.Value.Destinations.First(d => d.Id.Equals(EventType.DeletedUser.GetDescription(), StringComparison.OrdinalIgnoreCase)).Queue;
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    protected async override Task ExecuteAsync(CancellationToken cancellationToken)
    {
        using IServiceScope scope = _serviceProvider.CreateScope();
        IMessagingConnectionFactory factory = scope.ServiceProvider.GetRequiredService<IMessagingConnectionFactory>();
        IChannel channel = await factory.CreateChannelAsync(cancellationToken);
        await channel.BasicQosAsync(prefetchSize: 0, prefetchCount: 10, global: false, cancellationToken);
        AsyncEventingBasicConsumer consumer = new(channel);

        consumer.ReceivedAsync += async (_, ea) =>
        {
            using (LogContext.PushProperty("CorrelationId", ea.BasicProperties.CorrelationId))
            {
                string message = Encoding.UTF8.GetString(ea.Body.ToArray());

                try
                {
                    message = Encoding.UTF8.GetString(ea.Body.ToArray());
                    DeletedUserEvent? deletedUserEvent = JsonSerializer.Deserialize<DeletedUserEvent>(message);
                    if (deletedUserEvent is null)
                    {
                        Log.Error("Invalid DeletedUserEvent message: {Message}", message);
                        await channel.BasicAckAsync(ea.DeliveryTag, false);
                        return;
                    }

                    using IServiceScope scope = _serviceProvider.CreateScope();
                    IUnitOfWork unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

                    await unitOfWork.BeginTransactionAsync(cancellationToken);
                    await unitOfWork.Properties.DeleteAllPropertiesFromUserIdAsync(deletedUserEvent.UserId, cancellationToken);
                    await unitOfWork.CommitAsync(cancellationToken);

                    await channel.BasicAckAsync(ea.DeliveryTag, false);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "Error processing DeletedUserEvent with Message {Message}", message);
                    await channel.BasicNackAsync(ea.DeliveryTag, false, requeue: false);
                }
            }
        };

        await channel.BasicConsumeAsync(_queue, false, consumer, cancellationToken: cancellationToken);
    }
}
