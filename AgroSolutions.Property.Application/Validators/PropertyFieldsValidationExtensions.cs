using FluentValidation;

namespace AgroSolutions.Property.Application.Validators;

public static class PropertyFieldsValidationExtensions
{
    public const string MESSAGE_INVALID_PROPERTYID = "The property identifier is invalid";
    public const string MESSAGE_EMPTY_PROPERTYNAME = "The property name needs to be provided";
    public const string MESSAGE_LENGTH_PROPERTYNAME = "The property name cannot exceed 60 characters";
    public const string MESSAGE_LENGTH_PROPERTYDESCRIPTION = "The property description cannot exceed 500 characters";
    public const string MESSAGE_EMPTY_PROPERTYFIELDS = "At least one field is required";

    extension<T>(IRuleBuilder<T, int> rule)
    {
        public IRuleBuilderOptions<T, int> ValidPropertyId() => rule
            .GreaterThan(0).WithMessage(MESSAGE_INVALID_PROPERTYID);
    }

    extension<T>(IRuleBuilder<T, string> rule)
    {
        public IRuleBuilderOptions<T, string> ValidPropertyName() => rule
            .NotEmpty().WithMessage(MESSAGE_EMPTY_PROPERTYNAME)
            .MaximumLength(60).WithMessage(MESSAGE_LENGTH_PROPERTYNAME);
    }

    extension<T>(IRuleBuilder<T, string?> rule)
    {
        public IRuleBuilderOptions<T, string?> ValidPropertyDescriptionIfNotNull() => rule
            .MaximumLength(60).When((_, value) => value is not null)
            .WithMessage(MESSAGE_LENGTH_PROPERTYDESCRIPTION);
    }

    extension<T>(IRuleBuilder<T, IEnumerable<object>> rule)
    {
        public IRuleBuilderOptions<T, IEnumerable<object>?> ValidPropertyFieldsList() => rule
            .NotEmpty().WithMessage(MESSAGE_EMPTY_PROPERTYFIELDS);
    }
}
