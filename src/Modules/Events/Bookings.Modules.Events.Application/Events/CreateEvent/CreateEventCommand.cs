using Bookings.Modules.Events.Application.Abstractions.Messaging;

namespace Bookings.Modules.Events.Application.Events.CreateEvent;

public sealed record CreateEventCommand
    (string Title, Guid CategoryId, string Description, string Location, DateTime StartsAtUtc, DateTime? EndsAtUtc) 
    : ICommand<Guid>;
