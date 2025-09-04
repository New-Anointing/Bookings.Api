using Bookings.Common.Application.Messaging;

namespace Bookings.Modules.Events.Application.Events.CancelEvent;

public sealed record CancelEventCommand(Guid EventId) : ICommand;
