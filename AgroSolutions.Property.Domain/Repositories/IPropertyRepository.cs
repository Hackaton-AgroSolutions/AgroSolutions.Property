namespace AgroSolutions.Property.Domain.Repositories;

public interface IPropertyRepository
{
    Task<bool> CheckIfPropertyFieldNameExistsAsync(int propertyId, int userId, string fieldName, CancellationToken cancellationToken);
    Task<bool> CheckIfPropertyNameExistsAsync(string propertyName, CancellationToken cancellationToken);
    Task CreatePropertyAsync(Entities.Property property, CancellationToken cancellationToken);
    Task DeleteAllPropertiesFromUserIdAsync(int userId, CancellationToken cancellationToken);
    void DeleteProperty(Entities.Property property);
    Task<List<Entities.Property>> GetAllPropertiesWithFieldsAndCropByUserIdNoTrackingAsync(int userId, CancellationToken cancellationToken);
    Task<Entities.Property?> GetPropertyByIdAndUserIdTrackingAsync(int propertyId, int userId, CancellationToken cancellationToken);
    Task<Entities.Property?> GetPropertyByIdAndUserIdWithFieldsTrackingAsync(int propertyId, int userId, CancellationToken cancellationToken);
    Task<Entities.Property?> GetPropertyByIdAndUserIdWithFieldsAndCropNoTrackingAsync(int propertyId, int userId, CancellationToken cancellationToken);
}
