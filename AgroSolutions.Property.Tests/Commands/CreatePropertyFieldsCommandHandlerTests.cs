using AgroSolutions.Property.Application.Commands.CreatePropertyFields;
using AgroSolutions.Property.Application.Queries.GetProperty;
using AgroSolutions.Property.Domain.Entities;
using AgroSolutions.Property.Domain.Notifications;
using AgroSolutions.Property.Infrastructure.Persistence;
using FluentAssertions;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Moq;
using static Azure.Core.HttpHeader;

namespace AgroSolutions.Property.Tests.Commands;

public class CreatePropertyFieldsCommandHandlerTests
{
    private readonly Mock<INotificationContext> _notificationContext = new();
    private readonly Mock<IUnitOfWork> _unitOfWork = new();
    private readonly CreatePropertyFieldsCommandHandler _commandHandler;

    public CreatePropertyFieldsCommandHandlerTests()
    {
        _commandHandler = new(_notificationContext.Object, _unitOfWork.Object);
    }

    [Fact(DisplayName = "Should register fields and return all fields when property exists, fields names are unique and crops id was found")]
    public async Task Should_RegisterUserAndReturnToken_WhenEmailIsAvailable()
    {
        // Arrange
        Crop cropDb = new(1, "Coffe");
        Domain.Entities.Property propertyDb = new(1, "Valid and Unique Property Name", "Valid Property Description");
        CreatePropertyFieldsCommand createPropertyFieldsCommand = new(1, 2, [
            new("Valid and Unique Field Name", 1, 200)
        ]);
        _unitOfWork.Setup(u => u.Properties.GetPropertyByIdAndUserIdWithFieldsTrackingAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(propertyDb);
        _unitOfWork.Setup(u => u.Properties.CheckIfPropertyFieldNameExistsAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(false);
        _unitOfWork.Setup(u => u.Crops.GetByIdNoTrackingAsync(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(cropDb);

        // Act
        IEnumerable<CreatePropertyFieldsCommandResult>? createPropertyFieldsCommandResults = await _commandHandler.Handle(createPropertyFieldsCommand, CancellationToken.None);

        // Assert
        createPropertyFieldsCommandResults.Should().NotBeNull();
        createPropertyFieldsCommandResults.Count().Should().Be(1);
        createPropertyFieldsCommandResults.ElementAt(0).Name.Should().Be(createPropertyFieldsCommand.Fields.ElementAt(0).Name);
        createPropertyFieldsCommandResults.ElementAt(0).TotalAreaInHectares.Should().Be(createPropertyFieldsCommand.Fields.ElementAt(0).TotalAreaInHectares);
        createPropertyFieldsCommandResults.ElementAt(0).Crop.CropId.Should().Be(cropDb.CropId);
        createPropertyFieldsCommandResults.ElementAt(0).Crop.Name.Should().Be(cropDb.Name);
        _unitOfWork.Verify(u => u.Properties.GetPropertyByIdAndUserIdWithFieldsTrackingAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWork.Verify(u => u.Crops.GetByIdNoTrackingAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWork.Verify(u => u.Properties.CheckIfPropertyFieldNameExistsAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
        _notificationContext.Verify(n => n.AddNotification(It.IsAny<NotificationType>(), It.IsAny<IEnumerable<object>>()), Times.Never);
    }

    [Fact(DisplayName = "Should return null and Notify when property was not found")]
    public async Task Should_ReturnNullAndNotify_WhenPropertyWasNotFound()
    {
        // Arrange
        CreatePropertyFieldsCommand createPropertyFieldsCommand = new(1, 2, [
            new("First Valid and Unique Field Name", 1, 200),
            new("Second Valid and Unique Field Name", 1, 200)
        ]);
        _unitOfWork.Setup(u => u.Properties.GetPropertyByIdAndUserIdWithFieldsTrackingAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync((Domain.Entities.Property?)null);

        // Act
        IEnumerable<CreatePropertyFieldsCommandResult>? createPropertyFieldsCommandResults = await _commandHandler.Handle(createPropertyFieldsCommand, CancellationToken.None);

        // Assert
        createPropertyFieldsCommandResults.Should().BeNull();
        _unitOfWork.Verify(u => u.Properties.GetPropertyByIdAndUserIdWithFieldsTrackingAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Once);
        _notificationContext.Verify(n => n.AddNotification(It.IsAny<NotificationType>(), It.IsAny<IEnumerable<object>>()), Times.Once);
    }

    [Fact(DisplayName = "Should return null and Notify when crop was not found")]
    public async Task Should_ReturnNullAndNotify_WhenCropWasNotFound()
    {
        // Arrange
        Domain.Entities.Property propertyDb = new(1, "Valid and Unique Property Name", "Valid Property Description");
        CreatePropertyFieldsCommand createPropertyFieldsCommand = new(1, 2, [
            new("Valid and Unique Field Name", 1, 200)
        ]);
        _unitOfWork.Setup(u => u.Properties.GetPropertyByIdAndUserIdWithFieldsTrackingAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(propertyDb);
        _unitOfWork.Setup(u => u.Crops.GetByIdNoTrackingAsync(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync((Crop?)null);

        // Act
        IEnumerable<CreatePropertyFieldsCommandResult>? createPropertyFieldsCommandResults = await _commandHandler.Handle(createPropertyFieldsCommand, CancellationToken.None);

        // Assert
        createPropertyFieldsCommandResults.Should().BeNull();
        _unitOfWork.Verify(u => u.Properties.GetPropertyByIdAndUserIdWithFieldsTrackingAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWork.Verify(u => u.Crops.GetByIdNoTrackingAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWork.Verify(u => u.Properties.CheckIfPropertyFieldNameExistsAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
        _notificationContext.Verify(n => n.AddNotification(It.IsAny<NotificationType>(), It.IsAny<IEnumerable<object>>()), Times.Once);
    }

    [Fact(DisplayName = "Should return null and Notify when fields names are not unique")]
    public async Task Should_ReturnNullAndNotify_WhenFieldsNameAreNotUnique()
    {
        // Arrange
        Crop cropDb = new(1, "Coffe");
        Domain.Entities.Property propertyDb = new(1, "Valid and Unique Property Name", "Valid Property Description");
        CreatePropertyFieldsCommand createPropertyFieldsCommand = new(1, 2, [
            new("Valid and Unique Field Name", 1, 200)
        ]);
        _unitOfWork.Setup(u => u.Properties.GetPropertyByIdAndUserIdWithFieldsTrackingAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(propertyDb);
        _unitOfWork.Setup(u => u.Properties.CheckIfPropertyFieldNameExistsAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);
        _unitOfWork.Setup(u => u.Crops.GetByIdNoTrackingAsync(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(cropDb);

        // Act
        IEnumerable<CreatePropertyFieldsCommandResult>? createPropertyFieldsCommandResults = await _commandHandler.Handle(createPropertyFieldsCommand, CancellationToken.None);

        // Assert
        createPropertyFieldsCommandResults.Should().BeNull();
        _unitOfWork.Verify(u => u.Properties.GetPropertyByIdAndUserIdWithFieldsTrackingAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWork.Verify(u => u.Crops.GetByIdNoTrackingAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWork.Verify(u => u.Properties.CheckIfPropertyFieldNameExistsAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
        _notificationContext.Verify(n => n.AddNotification(It.IsAny<NotificationType>(), It.IsAny<IEnumerable<object>>()), Times.Once);
    }


    [Fact(DisplayName = "Should register fields and return all fields and Notify when fields names are not unique and crop was not found")]
    public async Task Should_ReturnNullAndNotify_WhenFieldsNamesAreNotUniqueAndCropNotWasNotFound()
    {
        // Arrange
        Crop cropDb = new(1, "Coffe");
        Domain.Entities.Property propertyDb = new(1, "Valid and Unique Property Name", "Valid Property Description");
        CreatePropertyFieldsCommand createPropertyFieldsCommand = new(1, 2, [
            new("First Valid and Unique Field Name", 1, 200),
            new("Second Valid and not Unique Field Name", 1, 200),
            new("Second Valid and Unique Field Name but with a non-existent cultural code", 1, 200)
        ]);
        _unitOfWork.Setup(u => u.Properties.GetPropertyByIdAndUserIdWithFieldsTrackingAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(propertyDb);
        _unitOfWork.SetupSequence(u => u.Properties.CheckIfPropertyFieldNameExistsAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false)
            .ReturnsAsync(true)
            .ReturnsAsync(false);
        _unitOfWork.SetupSequence(u => u.Crops.GetByIdNoTrackingAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(cropDb)
            .ReturnsAsync(cropDb)
            .ReturnsAsync(It.IsAny<Crop?>());

        // Act
        IEnumerable<CreatePropertyFieldsCommandResult> createPropertyFieldsCommandResults = (await _commandHandler.Handle(createPropertyFieldsCommand, CancellationToken.None))!;

        // Assert
        createPropertyFieldsCommandResults.Should().NotBeNull();
        createPropertyFieldsCommandResults.Count().Should().Be(1);
        createPropertyFieldsCommandResults.ElementAt(0).Name.Should().Be(createPropertyFieldsCommand.Fields.ElementAt(0).Name);
        createPropertyFieldsCommandResults.ElementAt(0).TotalAreaInHectares.Should().Be(createPropertyFieldsCommand.Fields.ElementAt(0).TotalAreaInHectares);
        createPropertyFieldsCommandResults.ElementAt(0).Crop.CropId.Should().Be(cropDb.CropId);
        createPropertyFieldsCommandResults.ElementAt(0).Crop.Name.Should().Be(cropDb.Name);
        _unitOfWork.Verify(u => u.Properties.GetPropertyByIdAndUserIdWithFieldsTrackingAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWork.Verify(u => u.Crops.GetByIdNoTrackingAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Exactly(3));
        _unitOfWork.Verify(u => u.Properties.CheckIfPropertyFieldNameExistsAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Exactly(2));
        _notificationContext.Verify(n => n.AddNotification(It.IsAny<NotificationType>(), It.IsAny<IEnumerable<object>>()), Times.Exactly(2));
    }
}
