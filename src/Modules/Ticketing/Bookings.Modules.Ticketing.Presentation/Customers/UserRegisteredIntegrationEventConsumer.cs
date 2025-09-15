using Bookings.Common.Application.Exceptions;
using Bookings.Common.Domain;
using Bookings.Modules.Ticketing.Application.Customers.CreateCustomer;
using Bookings.Modules.Users.IntegrationEvents;
using MassTransit;
using MediatR;

namespace Bookings.Modules.Ticketing.Presentation.Customers;

public sealed class UserRegisteredIntegrationEventConsumer(ISender sender) : IConsumer<UserRegisteredIntegrationEvent>
{
    public async Task Consume(ConsumeContext<UserRegisteredIntegrationEvent> context)
    {
        Result result = await sender.Send(
            new CreateCustomerCommand(
                context.Message.UserId,
                context.Message.Email,
                context.Message.FirstName,
                context.Message.LastName));

        if (result.IsFailure)
        {
            throw new BookingsException(nameof(CreateCustomerCommand), result.Error);
        }
    }
}
