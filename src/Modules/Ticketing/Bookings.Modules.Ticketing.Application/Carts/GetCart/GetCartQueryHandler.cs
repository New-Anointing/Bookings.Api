using Bookings.Common.Application.Messaging;
using Bookings.Common.Domain;

namespace Bookings.Modules.Ticketing.Application.Carts.GetCart;

internal sealed class GetCartQueryHandler(CartService cartService) : IQueryHandler<GetCartQuery, Cart>
{
    public async Task<Result<Cart>> Handle(GetCartQuery query, CancellationToken cancellationToken)
    {
        return await cartService.GetAsync(query.CustomerId, cancellationToken);
    }
}
