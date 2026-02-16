using AgroSolutions.Property.Application.Validators;
using FluentValidation;
using Serilog;

namespace AgroSolutions.Property.Application.Commands.UpdatePropertyField;

public class UpdatePropertyFieldCommandValidator : AbstractValidator<UpdatePropertyFieldCommand>
{
    public UpdatePropertyFieldCommandValidator()
    {
        Log.Information("Starting the validator {ValidatorName}.", nameof(UpdatePropertyFieldCommandValidator));

        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(c => c.UserId).ValidUserId();
        RuleFor(c => c.PropertyId).ValidPropertyId();
        RuleFor(c => c.FieldId).ValidFieldId();
        RuleFor(c => c.Name).ValidFieldName();
        RuleFor(c => c.CropId).ValidCropId();
        RuleFor(c => c.TotalAreaInHectares).ValidFieldTotalAreaInHectares();
    }
}
