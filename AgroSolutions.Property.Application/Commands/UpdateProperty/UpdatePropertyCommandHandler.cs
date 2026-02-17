using AgroSolutions.Property.Domain.Notifications;
using AgroSolutions.Property.Infrastructure.Persistence;
using MediatR;
using Serilog;

namespace AgroSolutions.Property.Application.Commands.UpdateProperty;

public class UpdatePropertyCommandHandler(INotificationContext notification, IUnitOfWork unitOfWork) : IRequestHandler<UpdatePropertyCommand, UpdatePropertyCommandResult?>
{
    private readonly INotificationContext _notification = notification;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<UpdatePropertyCommandResult?> Handle(UpdatePropertyCommand request, CancellationToken cancellationToken)
    {
        Log.Information("Starting the update of property.");

        Domain.Entities.Property? property = await _unitOfWork.Properties.GetPropertyByIdAndUserIdTrackingAsync(request.PropertyId, request.UserId, cancellationToken);// ?? throw new PropertyNotFoundException();
        if (property is null)
        {
            Log.Warning("The property with ID {PropertyId} from User with ID {UserId} was not found.", request.PropertyId, request.UserId);
            _notification.AddNotification(NotificationType.PropertyNotFound, [request.PropertyId]);
            return null;
        }

        if (await _unitOfWork.Properties.CheckIfPropertyNameExistsAsync(request.Name, cancellationToken))
        {
            Log.Warning("The property with Name {PropertyName} for User with ID {UserId} already exists.", request.Name, request.UserId);
            _notification.AddNotification(NotificationType.PropertyNameAlreadyExists, [request.Name, request.UserId]);
            return null;
        }

        Log.Information("Starting the process of updating the property with ID {PropertyId} in the database..", request.PropertyId);
        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        property.Update(request.Name, request.Description);
        await _unitOfWork.CommitAsync(cancellationToken);

        Log.Information("Finished the update of property.");
        return new(property.PropertyId, property.UserId, property.Name, property.Description);
    }
}
