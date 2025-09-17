using Bookings.Common.Application.Messaging;

namespace Bookings.Modules.Ticketing.Application.Carts.GetCart;

public sealed record GetCartQuery(Guid CustomerId) : IQuery<Cart>;
