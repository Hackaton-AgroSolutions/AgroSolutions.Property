using AgroSolutions.Property.Application.Validators;
using FluentValidation;
using Serilog;

namespace AgroSolutions.Property.Application.Commands.CreatePropertyFields;

public class CreatePropertyFieldsCommandValidator : AbstractValidator<CreatePropertyFieldsCommand>
{
    public CreatePropertyFieldsCommandValidator()
    {
        Log.Information("Starting the validator {ValidatorName}.", nameof(CreatePropertyFieldsCommandValidator));

        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(c => c.UserId).ValidUserId();
        RuleFor(c => c.PropertyId).ValidPropertyId();
        RuleFor(c => c.Fields).ValidPropertyFieldsList();
        RuleForEach(c => c.Fields)
            .ChildRules(f =>
            {
                RuleLevelCascadeMode = CascadeMode.Continue;
                f.RuleFor(f => f.Name).ValidFieldName();
                f.RuleFor(f => f.CropId).ValidCropId();
                f.RuleFor(f => f.TotalAreaInHectares).ValidFieldTotalAreaInHectares();
            });
    }
}
