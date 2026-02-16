using FluentValidation;

namespace AgroSolutions.Property.Application.Validators;

public static class CropFieldsValidationExtensions
{
    public const string MESSAGE_INVALID_CROPID = "The crop identifier is invalid";

    extension<T>(IRuleBuilder<T, int> rule)
    {
        public IRuleBuilderOptions<T, int> ValidCropId() => rule
            .GreaterThan(0).WithMessage(MESSAGE_INVALID_CROPID);
    }
}
