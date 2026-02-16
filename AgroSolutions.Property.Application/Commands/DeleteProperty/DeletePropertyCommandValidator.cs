using AgroSolutions.Property.Application.Validators;
using FluentValidation;
using Serilog;

namespace AgroSolutions.Property.Application.Commands.DeleteProperty;

public class DeletePropertyCommandValidator : AbstractValidator<DeletePropertyCommand>
{
    public DeletePropertyCommandValidator()
    {
        Log.Information("Starting the validator {ValidatorName}.", nameof(DeletePropertyCommandValidator));

        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(c => c.UserId).ValidUserId();
        RuleFor(c => c.PropertyId).ValidPropertyId();
    }
}
