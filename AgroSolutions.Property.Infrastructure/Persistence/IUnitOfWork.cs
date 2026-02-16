using AgroSolutions.Property.Domain.Repositories;

namespace AgroSolutions.Property.Infrastructure.Persistence;

public interface IUnitOfWork : IDisposable
{
    IPropertyRepository Properties { get; }
    ICropRepository Crops { get; }
    Task BeginTransactionAsync(CancellationToken cancellationToken);
    Task CommitAsync(CancellationToken cancellationToken);
}
