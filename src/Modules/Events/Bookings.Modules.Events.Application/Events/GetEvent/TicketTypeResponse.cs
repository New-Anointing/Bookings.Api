namespace Bookings.Modules.Events.Application.Events.GetEvent;

public sealed record TicketTypeResponse(
    Guid TicketTypeId,
    Guid EventId,
    string Name,
    decimal Price,
    string Currency,
    decimal Quantity);
