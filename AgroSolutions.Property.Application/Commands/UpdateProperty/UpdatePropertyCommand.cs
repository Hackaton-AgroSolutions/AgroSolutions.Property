using MediatR;

namespace AgroSolutions.Property.Application.Commands.UpdateProperty;

public record UpdatePropertyCommand(int UserId, int PropertyId, string Name, string? Description) : IRequest<UpdatePropertyCommandResult?>;
