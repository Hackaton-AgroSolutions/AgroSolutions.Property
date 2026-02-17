using AgroSolutions.Property.Domain.Entities;
using AgroSolutions.Property.Domain.Notifications;
using AgroSolutions.Property.Infrastructure.Persistence;
using MediatR;
using Serilog;

namespace AgroSolutions.Property.Application.Commands.DeletePropertyField;

public class DeletePropertyFieldCommandHandler(INotificationContext notification, IUnitOfWork unitOfWork) : IRequestHandler<DeletePropertyFieldCommand, Unit?>
{
    private readonly INotificationContext _notification = notification;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Unit?> Handle(DeletePropertyFieldCommand request, CancellationToken cancellationToken)
    {
        Log.Information("Started the property field deletion.");

        Domain.Entities.Property? property = await _unitOfWork.Properties.GetPropertyByIdAndUserIdWithFieldsTrackingAsync(request.PropertyId, request.UserId, cancellationToken);
        if (property is null)
        {
            Log.Warning("The property with ID {PropertyId} from User with ID {UserId} was not found.", request.PropertyId, request.UserId);
            _notification.AddNotification(NotificationType.PropertyNotFound, [request.PropertyId]);
            return null;
        }

        Field? field = property.Fields.FirstOrDefault(f => f.FieldId == request.FieldId);
        if (field is null)
        {
            Log.Warning("The field with ID {Field} from Property with ID {PropertyId} was not found.", request.FieldId, request.PropertyId);
            _notification.AddNotification(NotificationType.FieldNotFound, [request.FieldId]);
            return null;
        }

        Log.Information("Starting the process of deleting the field with ID {FieldId} in the database.", request.FieldId);
        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        property.Fields.Remove(field);
        await _unitOfWork.CommitAsync(cancellationToken);

        Log.Information("Finished the property field deletion.");
        return Unit.Value;
    }
}
