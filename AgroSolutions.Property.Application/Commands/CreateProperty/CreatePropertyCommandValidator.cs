using AgroSolutions.Property.Application.Validators;
using FluentValidation;
using Serilog;

namespace AgroSolutions.Property.Application.Commands.CreateProperty;

public class CreatePropertyCommandValidator : AbstractValidator<CreatePropertyCommand>
{
    public CreatePropertyCommandValidator()
    {
        Log.Information("Starting the validator {ValidatorName}.", nameof(CreatePropertyCommandValidator));

        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(c => c.UserId).ValidUserId();
        RuleFor(c => c.Name).ValidPropertyName();
        RuleFor(c => c.Description).ValidPropertyDescriptionIfNotNull();
    }
}
