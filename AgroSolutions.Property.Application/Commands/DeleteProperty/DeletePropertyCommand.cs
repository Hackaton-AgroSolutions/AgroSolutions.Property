using MediatR;

namespace AgroSolutions.Property.Application.Commands.DeleteProperty;

public record DeletePropertyCommand(int UserId, int PropertyId) : IRequest<Unit?>;
