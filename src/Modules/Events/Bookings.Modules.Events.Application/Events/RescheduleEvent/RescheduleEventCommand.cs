using Bookings.Modules.Events.Application.Abstractions.Messaging;

namespace Bookings.Modules.Events.Application.Events.RescheduleEvent;

public sealed record RescheduleEventCommand(Guid EventId, DateTime NewStartsAtUtc, DateTime? NewEndsAtUtc) : ICommand;
