using AgroSolutions.Property.Application.DTOs;

namespace AgroSolutions.Property.Application.Commands.UpdatePropertyField;

public record UpdatePropertyFieldCommandResult(int PropertyId, int UserId, int FieldId, string Name, CropUpinsertFieldDto Crop, decimal TotalAreaInHectares);
