using Bookings.Common.Application.Messaging;

namespace Bookings.Modules.Events.Application.Events.GetEvents;

public sealed record GetEventsQuery : IQuery<IReadOnlyCollection<EventResponse>>;
