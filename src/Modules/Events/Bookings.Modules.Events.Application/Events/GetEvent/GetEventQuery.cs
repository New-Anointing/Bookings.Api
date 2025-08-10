using Bookings.Shared.Abstractions.CQRS;

namespace Bookings.Modules.Events.Application.Events.GetEvent;

public sealed record GetEventQuery(Guid EventId) : IQuery<EventResponse>;
