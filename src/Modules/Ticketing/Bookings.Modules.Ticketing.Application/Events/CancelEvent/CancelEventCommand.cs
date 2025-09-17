using Bookings.Common.Application.Messaging;

namespace Bookings.Modules.Ticketing.Application.Events.CancelEvent;

public sealed record CancelEventCommand(Guid EventId) : ICommand;
