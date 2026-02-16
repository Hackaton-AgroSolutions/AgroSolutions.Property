using AgroSolutions.Property.Domain.Entities;
using AgroSolutions.Property.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AgroSolutions.Property.Infrastructure.Persistence.Repositories;

public class CropRepository(AgroSolutionsPropertyDbContext dbContext) : ICropRepository
{
    private readonly AgroSolutionsPropertyDbContext _dbContext = dbContext;

    public Task<Crop?> GetByIdNoTrackingAsync(int cropId, CancellationToken cancellationToken) => _dbContext.Crops
        .AsNoTracking()
        .FirstOrDefaultAsync(c => c.CropId == cropId, cancellationToken);

    public Task<List<Crop>> GetAllAsNoTrackingAsync(CancellationToken cancellationToken) => _dbContext.Crops
        .AsNoTracking()
        .ToListAsync(cancellationToken);
}

