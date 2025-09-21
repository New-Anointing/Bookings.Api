using Bookings.Common.Application.Messaging;

namespace Bookings.Modules.Ticketing.Application.Tickets.GetTicket;

public sealed record GetTicketQuery(Guid TicketId) : IQuery<TicketResponse>;
