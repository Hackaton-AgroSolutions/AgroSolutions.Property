using AgroSolutions.Property.Application.Validators;
using FluentValidation;
using Serilog;

namespace AgroSolutions.Property.Application.Commands.UpdateProperty;

public class UpdatePropertyCommandValidator : AbstractValidator<UpdatePropertyCommand>
{
    public UpdatePropertyCommandValidator()
    {
        Log.Information("Starting the validator {ValidatorName}.", nameof(UpdatePropertyCommandValidator));

        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(c => c.UserId).ValidUserId();
        RuleFor(c => c.PropertyId).ValidPropertyId();
        RuleFor(c => c.Name).ValidPropertyName();
        RuleFor(c => c.Description).ValidPropertyDescriptionIfNotNull();
    }
}
