using Bookings.Common.Application.Messaging;
using Bookings.Common.Domain;
using Bookings.Modules.Events.PublicApi;
using Bookings.Modules.Ticketing.Domain.Customers;
using Bookings.Modules.Ticketing.Domain.Events;
using FluentValidation;

namespace Bookings.Modules.Ticketing.Application.Carts.AddItemToCart;

public sealed record AddItemToCartCommand(Guid CustomerId, Guid TicketTypeId, decimal Quantity) : ICommand;

internal sealed class AddItemToCartCommandValidator : AbstractValidator<AddItemToCartCommand>
{
    public AddItemToCartCommandValidator()
    {
        RuleFor(c => c.CustomerId).NotEmpty();
        RuleFor(c => c.TicketTypeId).NotEmpty();
        RuleFor(c => c.Quantity).GreaterThan(decimal.Zero);
    }
}

internal sealed class AddItemToCartCommandHandler(CartService cartService, ICustomerRepository customerRepository, IEventsApi eventsApi)
    : ICommandHandler<AddItemToCartCommand>
{
    public async Task<Result> Handle(AddItemToCartCommand command, CancellationToken cancellationToken)
    {
        //1. Get Customer

        Customer? customer = await customerRepository.GetAsync(command.CustomerId, cancellationToken);

        if (customer is null)
        {
            return Result.Failure(CustomerErrors.NotFound(command.CustomerId));
        }

        //2. Get Ticket Type
        TicketTypeResponse? ticketType = await eventsApi.GetTicketTypeAsync(command.TicketTypeId, cancellationToken);

        if (ticketType is null)
        {
            return Result.Failure(TicketTypeErrors.NotFound(command.TicketTypeId));
        }

        var cartItem = new CartItem()
        {
            TicketTypeId = ticketType.Id,
            Quantity = command.Quantity,
            Price = ticketType.Price,
            Currency = ticketType.Currency,
        };

        //3. Add item to cart
        await cartService.AddItemAsync(customer.Id, cartItem, cancellationToken);

        return Result.Success();

    }
}
