using System.ComponentModel;

namespace AgroSolutions.Property.Domain.Messaging;

public enum EventType : byte
{
    [Description("DELETED_USER")] DeletedUser
}
