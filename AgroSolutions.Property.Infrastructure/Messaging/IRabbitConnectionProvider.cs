using RabbitMQ.Client;

namespace AgroSolutions.Property.Infrastructure.Messaging;

public interface IRabbitConnectionProvider
{
    Task<IConnection> GetConnectionAsync(CancellationToken cancellationToken);
}
