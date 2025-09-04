using Bookings.Common.Application.Messaging;

namespace Bookings.Modules.Events.Application.TicketTypes.CreateTicketType;

public sealed record CreateTicketTypeCommand
    (Guid EventId, string Name, decimal Price, string Currency, decimal Quantity) : ICommand<Guid>;
