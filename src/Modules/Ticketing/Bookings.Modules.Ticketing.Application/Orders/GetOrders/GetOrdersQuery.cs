using Bookings.Common.Application.Messaging;

namespace Bookings.Modules.Ticketing.Application.Orders.GetOrders;

public sealed record GetOrdersQuery(Guid CustomerId) : IQuery<IReadOnlyCollection<OrderResponse>>;
