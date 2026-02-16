using AgroSolutions.Property.Application.Queries.GetProperty;
using AgroSolutions.Property.Domain.Entities;
using AgroSolutions.Property.Domain.Notifications;
using AgroSolutions.Property.Infrastructure.Persistence;
using FluentAssertions;
using Moq;

namespace AgroSolutions.Property.Tests.Queries;

public class GetPropertyQueryHandlerTests
{
    private readonly Mock<INotificationContext> _notificationContext = new();
    private readonly Mock<IUnitOfWork> _unitOfWork = new();
    private readonly GetPropertyQueryHandler _queryHandler;

    public GetPropertyQueryHandlerTests()
    {
        _queryHandler = new(_notificationContext.Object, _unitOfWork.Object);
    }

    [Fact(DisplayName = "Should return property when it exists")]
    public async Task Should_ReturnProperty_WhenItExists()
    {
        // Arrange
        GetPropertyQuery getPropertyQuery = new(1, 2);
        Domain.Entities.Property propertyDb = new(1, "Valid and Unique Property Name", "Valid Property Description");
        propertyDb.Fields.Add(new("Valid and Unique Field Name", new(1, "Coffe"), 100));
        _unitOfWork.Setup(u => u.Properties.GetPropertyByIdAndUserIdWithFieldsAndCropNoTrackingAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(propertyDb);

        // Act
        GetPropertyQueryResult getPropertyQueryResult = (await _queryHandler.Handle(getPropertyQuery, CancellationToken.None))!;

        // Assert
        getPropertyQueryResult.Should().NotBeNull();
        getPropertyQueryResult.PropertyId.Should().Be(propertyDb.PropertyId);
        getPropertyQueryResult.UserId.Should().Be(propertyDb.UserId);
        getPropertyQueryResult.Name.Should().Be(propertyDb.Name);
        getPropertyQueryResult.Description.Should().Be(propertyDb.Description);
        getPropertyQueryResult.Fields.Count().Should().Be(1);
        _unitOfWork.Verify(u => u.Properties.GetPropertyByIdAndUserIdWithFieldsAndCropNoTrackingAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Once);
        _notificationContext.Verify(n => n.AddNotification(It.IsAny<NotificationType>()), Times.Never);
    }

    [Fact(DisplayName = "Should return null and Notify when property not exists in database")]
    public async Task Should_ReturnNullAndNotify_WhenPropertyNotExistsInDatabase()
    {
        // Arrange
        GetPropertyQuery getPropertyQuery = new(1, 2);
        _unitOfWork.Setup(u => u.Properties.GetPropertyByIdAndUserIdWithFieldsAndCropNoTrackingAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync((Domain.Entities.Property?)null);

        // Act
        GetPropertyQueryResult? getPropertyQueryResult = await _queryHandler.Handle(getPropertyQuery, CancellationToken.None);

        // Assert
        getPropertyQueryResult.Should().BeNull();
        _unitOfWork.Verify(u => u.Properties.GetPropertyByIdAndUserIdWithFieldsAndCropNoTrackingAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Once);
        _notificationContext.Verify(n => n.AddNotification(It.IsAny<NotificationType>(), It.IsAny<IEnumerable<object>>()), Times.Once);
    }
}
