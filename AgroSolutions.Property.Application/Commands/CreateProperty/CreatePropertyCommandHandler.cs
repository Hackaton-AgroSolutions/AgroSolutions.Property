using AgroSolutions.Property.Domain.Notifications;
using AgroSolutions.Property.Infrastructure.Persistence;
using MediatR;
using Serilog;

namespace AgroSolutions.Property.Application.Commands.CreateProperty;

public class CreatePropertyCommandHandler(INotificationContext notification, IUnitOfWork unitOfWork) : IRequestHandler<CreatePropertyCommand, CreatePropertyCommandResult?>
{
    private readonly INotificationContext _notification = notification;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<CreatePropertyCommandResult?> Handle(CreatePropertyCommand request, CancellationToken cancellationToken)
    {
        Log.Information("Starting the creation of property for user.");

        if (await _unitOfWork.Properties.CheckIfPropertyNameExistsAsync(request.Name, cancellationToken))
        {
            Log.Warning("The property with Name {PropertyName} for User with ID {UserId} already exists.", request.Name, request.UserId);
            _notification.AddNotification(NotificationType.PropertyNameAlreadyExists, [request.Name, request.UserId]);
            return null;
        }

        Log.Information("Adding the new user to the database.");
        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        Domain.Entities.Property property = new(request.UserId, request.Name, request.Description);
        await _unitOfWork.Properties.CreatePropertyAsync(property, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        Log.Information("Finished the creation of property for user.");
        return new(property.PropertyId, request.UserId, property.Name, property.Description);
    }
}
