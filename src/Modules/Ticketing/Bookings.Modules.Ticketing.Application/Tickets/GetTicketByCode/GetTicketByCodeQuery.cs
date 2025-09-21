using Bookings.Common.Application.Messaging;
using Bookings.Modules.Ticketing.Application.Tickets.GetTicket;

namespace Bookings.Modules.Ticketing.Application.Tickets.GetTicketByCode;

public sealed record GetTicketByCodeQuery(string Code) : IQuery<TicketResponse>;
