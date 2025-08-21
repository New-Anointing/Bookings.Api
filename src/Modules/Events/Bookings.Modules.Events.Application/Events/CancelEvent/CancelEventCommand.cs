using Bookings.Modules.Events.Application.Abstractions.Messaging;

namespace Bookings.Modules.Events.Application.Events.CancelEvent;

public sealed record CancelEventCommand(Guid EventId) : ICommand;
