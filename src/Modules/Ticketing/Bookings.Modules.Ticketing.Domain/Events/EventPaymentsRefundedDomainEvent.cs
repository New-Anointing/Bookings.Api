using Bookings.Common.Domain;

namespace Bookings.Modules.Ticketing.Domain.Events;

public sealed class EventPaymentsRefundedDomainEvent(Guid eventId) : DomainEvent
{
    public Guid EventId { get; init; } = eventId;
}

