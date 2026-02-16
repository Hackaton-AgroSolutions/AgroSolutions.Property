using MediatR;

namespace AgroSolutions.Property.Application.Commands.CreateProperty;

public record CreatePropertyCommand(int UserId, string Name, string? Description) : IRequest<CreatePropertyCommandResult?>;
