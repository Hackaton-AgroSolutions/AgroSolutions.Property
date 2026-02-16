using AgroSolutions.Property.Domain.Common;

namespace AgroSolutions.Property.Domain.Events;

public record DeletedUserEvent(int UserId) : IDomainEvent;
