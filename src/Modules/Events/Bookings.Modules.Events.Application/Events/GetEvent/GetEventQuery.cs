using Bookings.Modules.Events.Application.Abstractions.Messaging;

namespace Bookings.Modules.Events.Application.Events.GetEvent;

public sealed record GetEventQuery(Guid EventId) : IQuery<EventResponse>;
