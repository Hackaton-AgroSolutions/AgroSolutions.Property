using AgroSolutions.Property.Application.Commands.DeletePropertyField;
using AgroSolutions.Property.Application.Validators;
using FluentAssertions;
using FluentValidation.Results;

namespace AgroSolutions.Property.Tests.Validators;

public class DeletePropertyFieldCommandValidatorTests
{
    [Fact(DisplayName = "Valid command should pass validation")]
    public void Should_BeValid_WhenCommandIsValid()
    {
        // Arrange
        DeletePropertyFieldCommand deletePropertyFieldCommand = new(1, 2, 3);

        // Act
        ValidationResult result = new DeletePropertyFieldCommandValidator().Validate(deletePropertyFieldCommand);

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Fact(DisplayName = "Invalid command should fail validation")]
    public void Should_BeInvalid_WhenCommandIsInvalid()
    {
        // Arrange
        DeletePropertyFieldCommand deletePropertyFieldCommand = new(1, -2, 0);

        // Act
        ValidationResult result = new DeletePropertyFieldCommandValidator().Validate(deletePropertyFieldCommand);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCount(2);
        result.Errors.Count(e => e.ErrorMessage == PropertyFieldsValidationExtensions.MESSAGE_INVALID_PROPERTYID).Should().Be(1);
        result.Errors.Count(e => e.ErrorMessage == FieldFieldsValidationExtensions.MESSAGE_INVALID_FIELDID).Should().Be(1);
    }
}
