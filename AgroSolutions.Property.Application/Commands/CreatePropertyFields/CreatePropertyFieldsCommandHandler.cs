using AgroSolutions.Property.Application.DTOs;
using AgroSolutions.Property.Domain.Entities;
using AgroSolutions.Property.Domain.Notifications;
using AgroSolutions.Property.Infrastructure.Persistence;
using MediatR;
using Serilog;

namespace AgroSolutions.Property.Application.Commands.CreatePropertyFields;

public class CreatePropertyFieldsCommandHandler(INotificationContext notification, IUnitOfWork unitOfWork) : IRequestHandler<CreatePropertyFieldsCommand, IEnumerable<CreatePropertyFieldsCommandResult>?>
{
    private readonly INotificationContext _notification = notification;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<IEnumerable<CreatePropertyFieldsCommandResult>?> Handle(CreatePropertyFieldsCommand request, CancellationToken cancellationToken)
    {
        int totalInserted = 0;
        Domain.Entities.Property? property = await _unitOfWork.Properties.GetPropertyByIdAndUserIdWithFieldsTrackingAsync(request.PropertyId, request.UserId, cancellationToken);
        if (property is null)
        {
            Log.Warning("The property with ID {PropertyId} from User with ID {UserId} was not found.", request.PropertyId, request.UserId);
            _notification.AddNotification(NotificationType.PropertyNotFound, [request.PropertyId]);
            return null;
        }

        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        foreach (CreatePropertyFieldData addFieldData in request.Fields)
        {
            Crop? crop = await _unitOfWork.Crops.GetByIdNoTrackingAsync(addFieldData.CropId, cancellationToken);
            if (crop is null)
            {
                Log.Warning("The crop with ID {CropId} was not found.", addFieldData.CropId);
                _notification.AddNotification(NotificationType.CropNotFound, [addFieldData.CropId]);
                continue;
            }

            if (await _unitOfWork.Properties.CheckIfPropertyFieldNameExistsAsync(request.PropertyId, request.UserId, addFieldData.Name, cancellationToken))
            {
                Log.Warning("The property field with name {FieldName} already exists.", addFieldData.Name);
                _notification.AddNotification(NotificationType.PropertyFieldNameAlreadyExists, [addFieldData.Name]);
                continue;
            }

            Field field = new(addFieldData.Name, crop, addFieldData.TotalAreaInHectares);

            Log.Information("Creating a new field for the property with ID {PropertyId}.", property.PropertyId);
            property.Fields.Add(field);
            totalInserted++;
        }
        await _unitOfWork.CommitAsync(cancellationToken);

        return totalInserted == 0
            ? null
            : property.Fields.Select(f => new CreatePropertyFieldsCommandResult(
                f.PropertyId,
                request.UserId,
                f.FieldId,
                f.Name,
                new CropUpinsertFieldDto(f.Crop.CropId, f.Crop.Name),
                f.TotalAreaInHectares
            ));
    }
}
