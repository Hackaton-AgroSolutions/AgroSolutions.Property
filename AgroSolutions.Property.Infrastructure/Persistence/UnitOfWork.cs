using AgroSolutions.Property.Domain.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace AgroSolutions.Property.Infrastructure.Persistence;

public class UnitOfWork(AgroSolutionsPropertyDbContext dbContext, IPropertyRepository properties, ICropRepository crops) : IUnitOfWork
{
    private readonly AgroSolutionsPropertyDbContext _dbContext = dbContext;
    private IDbContextTransaction? _transaction;

    public IPropertyRepository Properties => properties;
    public ICropRepository Crops => crops;

    public async Task BeginTransactionAsync(CancellationToken cancellationToken) => _transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);

    public async Task CommitAsync(CancellationToken cancellationToken)
    {
        try
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
            await _transaction!.CommitAsync(cancellationToken);
        }
        catch
        {
            await _transaction!.RollbackAsync(cancellationToken);
            throw;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
            _dbContext.Dispose();
    }
}
