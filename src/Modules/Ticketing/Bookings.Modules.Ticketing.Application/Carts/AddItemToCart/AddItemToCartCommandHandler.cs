using Bookings.Common.Application.Messaging;
using Bookings.Common.Domain;
using Bookings.Modules.Ticketing.Domain.Customers;
using Bookings.Modules.Ticketing.Domain.Events;

namespace Bookings.Modules.Ticketing.Application.Carts.AddItemToCart;

internal sealed class AddItemToCartCommandHandler
    (CartService cartService,
    ICustomerRepository customerRepository,
   ITicketTypeRepository ticketTypeRepository)
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
        TicketType? ticketType = await ticketTypeRepository.GetAsync(command.TicketTypeId, cancellationToken);

        if (ticketType is null)
        {
            return Result.Failure(TicketTypeErrors.NotFound(command.TicketTypeId));
        }

        if (ticketType.AvailableQuantity < command.Quantity)
        {
            return Result.Failure(TicketTypeErrors.NotEnoughQuantity(ticketType.AvailableQuantity));
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
