using AgroSolutions.Property.Application.Commands.UpdatePropertyField;
using AgroSolutions.Property.Application.Validators;
using FluentAssertions;
using FluentValidation.Results;

namespace AgroSolutions.Property.Tests.Validators;

public class UpdatePropertyFieldCommandValidatorTests
{
    [Fact(DisplayName = "Valid command should pass validation")]
    public void Should_BeValid_WhenCommandIsValid()
    {
        // Arrange
        UpdatePropertyFieldCommand updatePropertyFieldCommand = new(1, 2, 3, "New Unique Field Name", 1, 220);

        // Act
        ValidationResult result = new UpdatePropertyFieldCommandValidator().Validate(updatePropertyFieldCommand);

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Fact(DisplayName = "Invalid command should fail validation")]
    public void Should_BeInvalid_WhenCommandIsInvalid()
    {
        // Arrange
        UpdatePropertyFieldCommand updatePropertyFieldCommand = new(1, -2, 0, "New Unique Field Name", 1, 0);

        // Act
        ValidationResult result = new UpdatePropertyFieldCommandValidator().Validate(updatePropertyFieldCommand);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCount(3);
        result.Errors.Count(e => e.ErrorMessage == PropertyFieldsValidationExtensions.MESSAGE_INVALID_PROPERTYID).Should().Be(1);
        result.Errors.Count(e => e.ErrorMessage == FieldFieldsValidationExtensions.MESSAGE_INVALID_FIELDID).Should().Be(1);
        result.Errors.Count(e => e.ErrorMessage == FieldFieldsValidationExtensions.MESSAGE_INVALID_TOTALAREAINHECTARES).Should().Be(1);
    }
}
