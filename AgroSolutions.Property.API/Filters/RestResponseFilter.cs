using AgroSolutions.Property.API.Extensions;
using AgroSolutions.Property.API.Responses;
using AgroSolutions.Property.Domain.Notifications;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace AgroSolutions.Property.API.Filters;

public class RestResponseFilter(INotificationContext notification) : IAsyncResultFilter
{
    private readonly INotificationContext _notification = notification;

    public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
    {
        if (_notification.HasValidations)
        {
            context.Result = new BadRequestObjectResult(new RestResponseWithInvalidFields { InvalidFields = _notification.Validations });
            await next();
            return;
        }

        if ((context.Result is NoContentResult || context.Result is AcceptedResult) && _notification.HasNotifications)
        {
            context.Result = new ObjectResult(new RestResponse { Notifications = _notification.AsListString })
            {
                StatusCode = MapStatusCode(_notification.Notifications)
            };
            await next();
            return;
        }

        if (context.Result is ObjectResult objectResult && objectResult.StatusCode >= 200 && objectResult.StatusCode < 300)
        {
            if (objectResult.Value is not null)
            {
                RestResponse restResponse = new(objectResult.Value);

                if (_notification.HasNotifications)
                {
                    objectResult.StatusCode = (int)HttpStatusCode.MultiStatus;
                    objectResult.Value = restResponse with { Notifications = _notification.AsListString };
                }
                else
                    objectResult.Value = restResponse;
            }
            else
            {
                context.Result = new ObjectResult(new RestResponse { Notifications = _notification.AsListString })
                {
                    StatusCode = MapStatusCode(_notification.Notifications)
                };
            }
        }

        await next();
    }

    private static int MapStatusCode(IReadOnlyCollection<Notification> notifications)
    {
        if (notifications.Any(n => n.Type == NotificationType.PropertyNotFound))
            return StatusCodes.Status404NotFound;

        if (notifications.Any(n => n.Type == NotificationType.FieldNotFound))
            return StatusCodes.Status404NotFound;

        if (notifications.Any(n => n.Type == NotificationType.CropNotFound))
            return StatusCodes.Status404NotFound;

        if (notifications.Any(n => n.Type == NotificationType.PropertyNameAlreadyExists))
            return StatusCodes.Status409Conflict;

        if (notifications.Any(n => n.Type == NotificationType.PropertyFieldNameAlreadyExists))
            return StatusCodes.Status409Conflict;

        return StatusCodes.Status400BadRequest;
    }
}
