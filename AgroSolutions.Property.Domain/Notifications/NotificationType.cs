using System.ComponentModel;

namespace AgroSolutions.Property.Domain.Notifications;

public enum NotificationType : byte
{
    [Description("The field with ID {0} was not found")] FieldNotFound,
    [Description("The crop with ID {0} was not found")] CropNotFound,
    [Description("The property with ID {0} was not found")] PropertyNotFound,
    [Description("The property with name '{0}' already exists")] PropertyNameAlreadyExists,
    [Description("The property field with name '{0}' already exists")]  PropertyFieldNameAlreadyExists
}
