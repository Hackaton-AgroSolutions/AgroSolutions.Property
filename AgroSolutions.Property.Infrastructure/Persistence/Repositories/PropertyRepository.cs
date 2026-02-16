using AgroSolutions.Property.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AgroSolutions.Property.Infrastructure.Persistence.Repositories;

public class PropertyRepository(AgroSolutionsPropertyDbContext dbContext) : IPropertyRepository
{
    private readonly AgroSolutionsPropertyDbContext _dbContext = dbContext;

    public Task<bool> CheckIfPropertyFieldNameExistsAsync(int propertyId, int userId, string fieldName, CancellationToken cancellationToken) => _dbContext.Properties
        .AsNoTracking()
        .Include(p => p.Fields.Where(f => f.PropertyId == propertyId))
        .AnyAsync(p => p.PropertyId == propertyId &&
                       p.UserId == userId &&
                       p.Fields.Any(f => f.Name == fieldName), cancellationToken);

    public Task<bool> CheckIfPropertyNameExistsAsync(string propertyName, CancellationToken cancellationToken) => _dbContext.Properties
        .AnyAsync(p => p.Name.ToLower() == propertyName.ToLower(), cancellationToken);

    public async Task CreatePropertyAsync(Domain.Entities.Property property, CancellationToken cancellationToken)
    {
        await _dbContext.Properties.AddAsync(property, cancellationToken);
    }

    public void DeleteProperty(Domain.Entities.Property property) => _dbContext.Properties.Remove(property);

    public Task DeleteAllPropertiesFromUserIdAsync(int userId, CancellationToken cancellationToken) => _dbContext.Properties
        .Where(p => p.UserId == userId)
        .ExecuteDeleteAsync(cancellationToken);

    public Task<List<Domain.Entities.Property>> GetAllPropertiesWithFieldsAndCropByUserIdNoTrackingAsync(int userId, CancellationToken cancellationToken) => _dbContext.Properties
        .AsNoTracking()
        .Where(p => p.UserId == userId)
        .Include(p => p.Fields)
        .ThenInclude(f => f.Crop)
        .ToListAsync(cancellationToken);

    public Task<Domain.Entities.Property?> GetPropertyByIdAndUserIdTrackingAsync(int propertyId, int userId, CancellationToken cancellationToken) => _dbContext.Properties
        .FirstOrDefaultAsync(p => p.PropertyId == propertyId && p.UserId == userId, cancellationToken);

    public Task<Domain.Entities.Property?> GetPropertyByIdAndUserIdWithFieldsTrackingAsync(int propertyId, int userId, CancellationToken cancellationToken) => _dbContext.Properties
        .Include(p => p.Fields)
        .FirstOrDefaultAsync(p => p.PropertyId == propertyId && p.UserId == userId, cancellationToken);

    public Task<Domain.Entities.Property?> GetPropertyByIdAndUserIdWithFieldsAndCropNoTrackingAsync(int propertyId, int userId, CancellationToken cancellationToken) => _dbContext.Properties
        .AsNoTracking()
        .Include(p => p.Fields)
        .ThenInclude(f => f.Crop)
        .FirstOrDefaultAsync(p => p.PropertyId == propertyId && p.UserId == userId, cancellationToken);
}
