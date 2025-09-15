using Bookings.Common.Application.EventBus;
using Bookings.Common.Application.Exceptions;
using Bookings.Common.Application.Messaging;
using Bookings.Common.Domain;
using Bookings.Modules.Users.Application.Users.GetUser;
using Bookings.Modules.Users.Domain.Users;
using Bookings.Modules.Users.IntegrationEvents;
using MediatR;

namespace Bookings.Modules.Users.Application.Users.RegisterUser;

internal sealed class UserRegisteredDomainEventHandler(ISender sender, IEventBus eventBus) : IDomainEventHandler<UserRegisteredDomainEvent>
{
    public async Task Handle(UserRegisteredDomainEvent notification, CancellationToken cancellationToken)
    {
        Result<UserResponse> response = await sender.Send(new GetUserQuery(notification.UserId), cancellationToken);

        if (response.IsFailure)
        {
            throw new BookingsException(nameof(GetUserQuery), response.Error);
        }

        await eventBus.PublishAsync(
            new UserRegisteredIntegrationEvent(
                notification.Id,
                notification.OccuredOnUtc,
                response.Value.Id,
                response.Value.Email,
                response.Value.FirstName,
                response.Value.LastName),
            cancellationToken);
    }
}
