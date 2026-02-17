using AgroSolutions.Property.Application.DTOs;
using AgroSolutions.Property.Domain.Entities;
using AgroSolutions.Property.Domain.Notifications;
using AgroSolutions.Property.Infrastructure.Persistence;
using MediatR;
using Serilog;

namespace AgroSolutions.Property.Application.Commands.UpdatePropertyField;

public class UpdatePropertyFieldCommandHandler(INotificationContext notification, IUnitOfWork unitOfWork) : IRequestHandler<UpdatePropertyFieldCommand, UpdatePropertyFieldCommandResult?>
{
    private readonly INotificationContext _notification = notification;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<UpdatePropertyFieldCommandResult?> Handle(UpdatePropertyFieldCommand request, CancellationToken cancellationToken)
    {
        Log.Information("Started the update of property field.");

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

        Crop? crop = await _unitOfWork.Crops.GetByIdNoTrackingAsync(request.CropId, cancellationToken);
        if (crop is null)
        {
            Log.Warning("The crop with ID {CropId} was not found.", request.CropId);
            _notification.AddNotification(NotificationType.CropNotFound, [request.CropId]);
            return null;
        }

        Log.Information("Starting the process of updating the field with ID {FieldId} in the database..", request.FieldId);
        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        field.Update(request.Name, request.TotalAreaInHectares, crop);
        await _unitOfWork.CommitAsync(cancellationToken);

        Log.Information("Finished the update of property field.");
        return new(
            field.PropertyId,
            request.UserId,
            field.FieldId,
            field.Name,
            new CropUpinsertFieldDto(field.Crop.CropId, field.Crop.Name),
            field.TotalAreaInHectares
        );
    }
}
