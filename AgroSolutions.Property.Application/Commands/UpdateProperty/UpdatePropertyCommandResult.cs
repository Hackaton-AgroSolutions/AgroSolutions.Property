namespace AgroSolutions.Property.Application.Commands.UpdateProperty;

public record UpdatePropertyCommandResult(int PropertyId, int UserId, string Name, string? Description);
