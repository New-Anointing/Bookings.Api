using Bookings.Common.Domain;

namespace Bookings.Modules.Ticketing.Domain.Events;

public class TicketTypeSoldOutDomainEvent(Guid ticketTypeId) : DomainEvent
{
    public Guid TicketTypeId { get; } = ticketTypeId;
}
