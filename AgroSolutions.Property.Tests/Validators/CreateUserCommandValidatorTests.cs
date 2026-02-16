using AgroSolutions.Property.Application.Commands.CreatePropertyFields;
using AgroSolutions.Property.Application.Validators;
using FluentAssertions;
using FluentValidation.Results;

namespace AgroSolutions.Property.Tests.Validators;

public class CreatePropertyFieldsCommandHandlerTests
{
    [Fact(DisplayName = "Valid command should pass validation")]
    public void Should_BeValid_WhenCommandIsValid()
    {
        // Arrange
        CreatePropertyFieldsCommand createPropertyFieldsCommand = new(1, 2, [
            new("First Valid Field Name", 3, 200),
            new("Second Valid Field Name", 4, 5200)
        ]);

        // Act
        ValidationResult result = new CreatePropertyFieldsCommandValidator().Validate(createPropertyFieldsCommand);

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().HaveCount(0);
    }

    [Fact(DisplayName = "Invalid command should fail validation")]
    public void Should_BeInvalid_WhenCommandIsInvalid()
    {
        // Arrange
        CreatePropertyFieldsCommand createPropertyFieldsCommand = new(-1, 0, [
            new("First Valid Field Name", 1, 200),
            new("Second Invalid Field Name with more than 60 characteres for tests", 1, -5200)
        ]);

        // Act
        ValidationResult result = new CreatePropertyFieldsCommandValidator().Validate(createPropertyFieldsCommand);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCount(4);
        result.Errors.Count(e => e.ErrorMessage == UserFieldsValidationExtensions.MESSAGE_INVALID_USERID).Should().Be(1);
        result.Errors.Count(e => e.ErrorMessage == PropertyFieldsValidationExtensions.MESSAGE_INVALID_PROPERTYID).Should().Be(1);
        result.Errors.Count(e => e.ErrorMessage == FieldFieldsValidationExtensions.MESSAGE_LENGTH_FIELDNAME).Should().Be(1);
        result.Errors.Count(e => e.ErrorMessage == FieldFieldsValidationExtensions.MESSAGE_INVALID_TOTALAREAINHECTARES).Should().Be(1);
    }

    [Fact(DisplayName = "Invalid crop id and total area in hectares should return crop id and total area in hectares validations error")]
    public void Should_HaveCropIdAndTotalAreaInHectaresError_WhenAnyCropIdAndTotalAreaInHectaresIsInvalid()
    {
        // Arrange
        CreatePropertyFieldsCommand createPropertyFieldsCommand = new(1, 2, [
            new("First Valid Field Name", 3, 200),
            new("Second Valid Field Name", -24, -2)
        ]);

        // Act
        ValidationResult result = new CreatePropertyFieldsCommandValidator().Validate(createPropertyFieldsCommand);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCount(2);
        result.Errors.Count(e => e.ErrorMessage == CropFieldsValidationExtensions.MESSAGE_INVALID_CROPID).Should().Be(1);
        result.Errors.Count(e => e.ErrorMessage == FieldFieldsValidationExtensions.MESSAGE_INVALID_TOTALAREAINHECTARES).Should().Be(1);
    }
}
