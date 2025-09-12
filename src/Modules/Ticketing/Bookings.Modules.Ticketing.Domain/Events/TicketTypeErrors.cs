using Bookings.Common.Domain;

namespace Bookings.Modules.Ticketing.Domain.Events;

public static class TicketTypeErrors
{
    public static Error NotFound(Guid ticketTypeId) =>
        Error.NotFound("TicketTypes.NotFound", $"The ticket type with the indentifier {ticketTypeId} was not found");

    public static Error NotEnoughQuantity(decimal availableQuantity) =>
       Error.Problem(
           "TicketTypes.NotEnoughQuantity",
           $"The ticket type has {availableQuantity} quantity available");
}
