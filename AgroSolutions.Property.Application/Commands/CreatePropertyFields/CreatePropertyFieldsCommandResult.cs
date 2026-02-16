using AgroSolutions.Property.Application.DTOs;

namespace AgroSolutions.Property.Application.Commands.CreatePropertyFields;

public record CreatePropertyFieldsCommandResult(int PropertyId, int UserId, int FieldId, string Name, CropUpinsertFieldDto Crop, decimal TotalAreaInHectares);
