using AgroSolutions.Property.Application.DTOs;

namespace AgroSolutions.Property.Application.Queries.GetProperties;

public record GetPropertiesQueryResult(int PropertyId, int UserId, string Name, string? Description, IEnumerable<FieldDto> Fields);
