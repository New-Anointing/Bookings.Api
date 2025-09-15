using Bookings.Common.Application.Messaging;

namespace Bookings.Modules.Events.Application.Events.CreateEvent;

public sealed record CreateEventCommand
    (string Title, Guid CategoryId, string Description, string Location, DateTime StartsAtUtc, DateTime? EndsAtUtc)
    : ICommand<Guid>;
