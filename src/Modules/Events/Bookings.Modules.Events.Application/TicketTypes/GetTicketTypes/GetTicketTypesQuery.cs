using Bookings.Modules.Events.Application.Abstractions.Messaging;
using Bookings.Modules.Events.Application.TicketTypes.GetTicketType;


namespace Bookings.Modules.Events.Application.TicketTypes.GetTicketTypes;

public sealed record GetTicketTypesQuery(Guid EventId) : IQuery<IReadOnlyCollection<TicketTypeResponse>>;
