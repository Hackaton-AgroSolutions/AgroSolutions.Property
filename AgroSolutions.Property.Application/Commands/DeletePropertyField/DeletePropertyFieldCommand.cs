using MediatR;

namespace AgroSolutions.Property.Application.Commands.DeletePropertyField;

public record DeletePropertyFieldCommand(int UserId, int PropertyId, int FieldId) : IRequest<Unit?>;
