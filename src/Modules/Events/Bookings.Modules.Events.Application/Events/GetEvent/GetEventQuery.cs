using Bookings.Common.Application.Messaging;

namespace Bookings.Modules.Events.Application.Events.GetEvent;

public sealed record GetEventQuery(Guid EventId) : IQuery<EventResponse>;
