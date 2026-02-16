using AgroSolutions.Property.Application.DTOs;

namespace AgroSolutions.Property.Application.Queries.GetProperty;

public record GetPropertyQueryResult(int PropertyId, int UserId, string Name, string? Description, IEnumerable<FieldDto> Fields);
