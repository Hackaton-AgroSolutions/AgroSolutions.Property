using AgroSolutions.Property.Domain.Entities;

namespace AgroSolutions.Property.Domain.Repositories;

public interface ICropRepository
{
    Task<Crop?> GetByIdNoTrackingAsync(int cropId, CancellationToken cancellationToken);
    Task<List<Crop>> GetAllAsNoTrackingAsync(CancellationToken cancellationToken);
}
