namespace AgroSolutions.Property.Application.Commands.CreateProperty;

public record CreatePropertyCommandResult(int PropertyId, int UserId, string Name, string? Description);
