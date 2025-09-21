using Bookings.Common.Application.Messaging;
using Bookings.Modules.Ticketing.Application.Tickets.GetTicket;

namespace Bookings.Modules.Ticketing.Application.Tickets.GetTicketForOrder;

public sealed record GetTicketsForOrderQuery(Guid OrderId) : IQuery<IReadOnlyCollection<TicketResponse>>;
