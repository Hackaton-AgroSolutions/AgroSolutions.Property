using RabbitMQ.Client;

namespace AgroSolutions.Property.Infrastructure.Messaging;

public interface IMessagingConnectionFactory
{
    Task<IChannel> CreateChannelAsync(CancellationToken cancellationToken);
}
