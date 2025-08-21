using Bookings.Modules.Events.Application.Abstractions.Messaging;

namespace Bookings.Modules.Events.Application.TicketTypes.UpdateTicketTypePrice;

public sealed record UpdateTicketTypePriceCommand(Guid TicketTypeId, decimal Price) : ICommand;
