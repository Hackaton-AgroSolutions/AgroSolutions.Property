using MediatR;

namespace AgroSolutions.Property.Application.Commands.UpdatePropertyField;

public record UpdatePropertyFieldCommand(int UserId, int PropertyId, int FieldId, string Name, int CropId, decimal TotalAreaInHectares) : IRequest<UpdatePropertyFieldCommandResult?>;
