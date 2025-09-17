using Bookings.Common.Application.Messaging;

namespace Bookings.Modules.Ticketing.Application.Events.RescheduleEvent;

public sealed record RescheduleEventCommand(Guid EventId, DateTime StartsAtUtc, DateTime? EndsAtUtc)
    : ICommand;
