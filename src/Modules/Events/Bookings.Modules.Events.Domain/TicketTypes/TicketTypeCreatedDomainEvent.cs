using Bookings.Modules.Events.Domain.Abstractions;

namespace Bookings.Modules.Events.Domain.TicketTypes;

public sealed class TicketTypeCreatedDomainEvent(Guid ticketTypeId) : DomainEvent
{
    public Guid TicketTypeId { get; init; } = ticketTypeId;
}
