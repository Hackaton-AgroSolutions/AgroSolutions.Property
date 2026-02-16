using AgroSolutions.Property.Application.Commands.DeletePropertyField;
using AgroSolutions.Property.Domain.Notifications;
using AgroSolutions.Property.Infrastructure.Persistence;
using FluentAssertions;
using MediatR;
using Moq;

namespace AgroSolutions.Property.Tests.Commands;

public class DeletePropertyFieldCommandHandlerTests
{
    private readonly Mock<INotificationContext> _notificationContext = new();
    private readonly Mock<IUnitOfWork> _unitOfWork = new();
    private readonly DeletePropertyFieldCommandHandler _commandHandler;

    public DeletePropertyFieldCommandHandlerTests()
    {
        _commandHandler = new(_notificationContext.Object, _unitOfWork.Object);
    }

    [Fact(DisplayName = "Should delete property field and return unit when property and field exists")]
    public async Task Should_DeletePropertyFieldAndReturnUnit_WhenPropertyAndFieldExits()
    {
        // Arrange
        DeletePropertyFieldCommand deletePropertyFieldCommand = new(1, 2, 3);
        Domain.Entities.Property propertyDb = new(1, "Valid Property Name", "Valid Property Description");
        propertyDb.Fields.Add(new(deletePropertyFieldCommand.FieldId, "Valid Field Name", new(1, "Coffe"), 200));
        _unitOfWork.Setup(u => u.Properties.GetPropertyByIdAndUserIdWithFieldsTrackingAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(propertyDb);

        // Act
        Unit? unit = await _commandHandler.Handle(deletePropertyFieldCommand, CancellationToken.None);

        // Assert
        unit.Should().NotBeNull();
        _notificationContext.Verify(n => n.AddNotification(It.IsAny<NotificationType>()), Times.Never);
        _unitOfWork.Verify(u => u.Properties.GetPropertyByIdAndUserIdWithFieldsTrackingAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWork.Verify(u => u.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact(DisplayName = "Should notify and return null when property is not found")]
    public async Task Should_ReturnNullAndNotify_WhenPropertyNotFound()
    {
        // Arrange
        DeletePropertyFieldCommand deletePropertyFieldCommand = new(1, 2, 3);
        _unitOfWork.Setup(u => u.Properties.GetPropertyByIdAndUserIdWithFieldsTrackingAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Domain.Entities.Property?)null);

        // Act
        Unit? unit = await _commandHandler.Handle(deletePropertyFieldCommand, CancellationToken.None);

        // Assert
        unit.Should().BeNull();
        _notificationContext.Verify(n => n.AddNotification(It.IsAny<NotificationType>(), It.IsAny<IEnumerable<object>?>()), Times.Once);
        _unitOfWork.Verify(u => u.Properties.GetPropertyByIdAndUserIdWithFieldsTrackingAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWork.Verify(u => u.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}
