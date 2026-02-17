using AgroSolutions.Property.Application.DTOs;
using AgroSolutions.Property.Domain.Notifications;
using AgroSolutions.Property.Infrastructure.Persistence;
using MediatR;
using Serilog;

namespace AgroSolutions.Property.Application.Queries.GetProperty;

public class GetPropertyQueryHandler(INotificationContext notification, IUnitOfWork unitOfWork) : IRequestHandler<GetPropertyQuery, GetPropertyQueryResult?>
{
    private readonly INotificationContext _notification = notification;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<GetPropertyQueryResult?> Handle(GetPropertyQuery request, CancellationToken cancellationToken)
    {
        Log.Information("Started the get property.");

        Domain.Entities.Property? property = await _unitOfWork.Properties.GetPropertyByIdAndUserIdWithFieldsAndCropNoTrackingAsync(request.PropertyId, request.UserId, cancellationToken);
        if (property is null)
        {
            Log.Warning("The property with ID {PropertyId} from User with ID {UserId} was not found.", request.PropertyId, request.UserId);
            _notification.AddNotification(NotificationType.PropertyNotFound, [request.PropertyId]);
            return null;
        }

        Log.Information("Finished the get property.");
        return new(
            property.PropertyId,
            property.UserId,
            property.Name,
            property.Description,
            property.Fields.Select(f => new FieldDto(
                f.FieldId,
                f.Name,
                f.Crop.Name,
                f.TotalAreaInHectares
            ))
        );
    }
}

