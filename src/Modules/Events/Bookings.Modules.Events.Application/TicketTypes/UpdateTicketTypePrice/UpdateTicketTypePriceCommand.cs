using Bookings.Common.Application.Messaging;

namespace Bookings.Modules.Events.Application.TicketTypes.UpdateTicketTypePrice;

public sealed record UpdateTicketTypePriceCommand(Guid TicketTypeId, decimal Price) : ICommand;
