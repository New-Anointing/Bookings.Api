using Bookings.Common.Domain;

namespace Bookings.Modules.Events.Domain.TicketTypes;

public sealed class TicketTypePriceChangeDomainEvent(Guid ticketTypeId, decimal price) : DomainEvent
{
    public Guid TicketTypeId { get; init; } = ticketTypeId;
    public decimal Price { get; init; } = price;
}
