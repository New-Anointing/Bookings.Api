using Bookings.Common.Domain;

namespace Bookings.Modules.Events.Domain.Events;

public sealed partial class Event
{
    public sealed class EventPublishedDomainEvent(Guid eventId) : DomainEvent
    {
        public Guid EventId { get; init; } = eventId;
    }
}

