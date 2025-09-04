using Bookings.Common.Application.Messaging;

namespace Bookings.Modules.Events.Application.Events.PublishEvent;

public sealed record PublishEventCommand(Guid EventId) : ICommand;
