using Bookings.Shared.Abstractions.CQRS;

namespace Bookings.Modules.Events.Application.Events.CreateEvent;

public sealed record CreateEventCommand
    (string Title, string Description, string Location, DateTime StartsAtUtc, DateTime? EndsAtUtc) 
    : ICommand<Guid>;
