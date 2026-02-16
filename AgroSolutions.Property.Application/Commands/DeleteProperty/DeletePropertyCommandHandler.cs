using AgroSolutions.Property.Domain.Notifications;
using AgroSolutions.Property.Infrastructure.Persistence;
using MediatR;
using Serilog;

namespace AgroSolutions.Property.Application.Commands.DeleteProperty;

public class DeletePropertyCommandHandler(INotificationContext notification, IUnitOfWork unitOfWork) : IRequestHandler<DeletePropertyCommand, Unit?>
{
    private readonly INotificationContext _notification = notification;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Unit?> Handle(DeletePropertyCommand request, CancellationToken cancellationToken)
    {
        Domain.Entities.Property? property = await _unitOfWork.Properties.GetPropertyByIdAndUserIdTrackingAsync(request.PropertyId, request.UserId, cancellationToken);
        if (property is null)
        {
            Log.Warning("The property with ID {PropertyId} from User with ID {UserId} was not found.", request.PropertyId, request.UserId);
            _notification.AddNotification(NotificationType.PropertyNotFound, [request.PropertyId]);
            return null;
        }

        Log.Information("Starting the process of deleting the property with ID {PropertyId} in the database.", property.PropertyId);
        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        _unitOfWork.Properties.DeleteProperty(property);
        await _unitOfWork.CommitAsync(cancellationToken);

        return Unit.Value;
    }
}
