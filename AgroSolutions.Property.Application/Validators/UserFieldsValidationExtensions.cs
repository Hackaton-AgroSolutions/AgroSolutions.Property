using FluentValidation;

namespace AgroSolutions.Property.Application.Validators;

public static class UserFieldsValidationExtensions
{
    public const string MESSAGE_INVALID_USERID = "The user identifier is invalid";

    extension<T>(IRuleBuilder<T, int> rule)
    {
        public IRuleBuilderOptions<T, int> ValidUserId() => rule
            .GreaterThan(0).WithMessage(MESSAGE_INVALID_USERID);
    }
}
