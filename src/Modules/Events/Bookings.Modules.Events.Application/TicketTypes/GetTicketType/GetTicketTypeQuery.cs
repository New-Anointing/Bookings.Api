using Bookings.Common.Application.Messaging;

namespace Bookings.Modules.Events.Application.TicketTypes.GetTicketType;

public sealed record GetTicketTypeQuery(Guid TicketTypeId) : IQuery<TicketTypeResponse>;
