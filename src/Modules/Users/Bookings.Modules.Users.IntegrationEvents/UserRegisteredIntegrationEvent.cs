using Bookings.Common.Application.EventBus;

namespace Bookings.Modules.Users.IntegrationEvents;

public sealed class UserRegisteredIntegrationEvent : IntegrationEvent
{
    public Guid UserId { get; init; }
    public string Email { get; init; }
    public string FirstName { get; init; }
    public string LastName { get; init; }

    public UserRegisteredIntegrationEvent(Guid id, DateTime occuredOnUtc, Guid userId, string email, string firstName, string lastName)
        : base(id, occuredOnUtc)
    {
        UserId = userId;
        Email = email;
        FirstName = firstName;
        LastName = lastName;
    }
}
