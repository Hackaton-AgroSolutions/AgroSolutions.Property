using FluentValidation;

namespace AgroSolutions.Property.Application.Validators;

public static class FieldFieldsValidationExtensions
{
    public const string MESSAGE_INVALID_FIELDID = "The field identifier is invalid";
    public const string MESSAGE_EMPTY_FIELDNAME = "The field name needs to be provided";
    public const string MESSAGE_LENGTH_FIELDNAME = "The field name cannot exceed 100 characters";
    public const string MESSAGE_INVALID_TOTALAREAINHECTARES = "The total area in hectares cannot be less than or equal to 0";

    extension<T>(IRuleBuilder<T, int> rule)
    {
        public IRuleBuilderOptions<T, int> ValidFieldId() => rule
            .GreaterThan(0).WithMessage(MESSAGE_INVALID_FIELDID);
    }

    extension<T>(IRuleBuilder<T, string> rule)
    {
        public IRuleBuilderOptions<T, string> ValidFieldName() => rule
            .NotEmpty().WithMessage(MESSAGE_EMPTY_FIELDNAME)
            .MaximumLength(60).WithMessage(MESSAGE_LENGTH_FIELDNAME);
    }

    extension<T>(IRuleBuilder<T, decimal> rule)
    {
        public IRuleBuilderOptions<T, decimal> ValidFieldTotalAreaInHectares() => rule
            .GreaterThan(0).WithMessage(MESSAGE_INVALID_TOTALAREAINHECTARES);
    }
}
