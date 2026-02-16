using AgroSolutions.Property.Application.Validators;
using FluentValidation;
using Serilog;

namespace AgroSolutions.Property.Application.Commands.DeletePropertyField;

public class DeletePropertyFieldCommandValidator : AbstractValidator<DeletePropertyFieldCommand>
{
    public DeletePropertyFieldCommandValidator()
    {
        Log.Information("Starting the validator {ValidatorName}.", nameof(DeletePropertyFieldCommandValidator));

        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(c => c.UserId).ValidUserId();
        RuleFor(c => c.PropertyId).ValidPropertyId();
        RuleFor(c => c.FieldId).ValidFieldId();
    }
}
