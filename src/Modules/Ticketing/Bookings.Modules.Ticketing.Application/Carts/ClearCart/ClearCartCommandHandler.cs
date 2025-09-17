using Bookings.Common.Application.Messaging;
using Bookings.Common.Domain;
using Bookings.Modules.Ticketing.Domain.Customers;

namespace Bookings.Modules.Ticketing.Application.Carts.ClearCart;

internal sealed class ClearCartCommandHandler(
    CartService cartService,
    ICustomerRepository customerRepository)
    : ICommandHandler<ClearCartCommand>
{
    public async Task<Result> Handle(ClearCartCommand command, CancellationToken cancellationToken)
    {
        Customer? customer = await customerRepository.GetAsync(command.CustomerId, cancellationToken);

        if (customer is null)
        {
            return Result.Failure(CustomerErrors.NotFound(command.CustomerId));
        }

        await cartService.ClearAsync(command.CustomerId, cancellationToken);

        return Result.Success();
    }
}
