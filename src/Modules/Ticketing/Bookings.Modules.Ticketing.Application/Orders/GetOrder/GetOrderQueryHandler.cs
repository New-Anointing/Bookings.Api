using System.Data.Common;
using Bookings.Common.Application.Data;
using Bookings.Common.Application.Messaging;
using Bookings.Common.Domain;
using Bookings.Modules.Ticketing.Domain.Orders;
using Dapper;

namespace Bookings.Modules.Ticketing.Application.Orders.GetOrder;

public sealed record GetOrderQuery(Guid OrderId) : IQuery<OrderResponse>;


public sealed record OrderResponse(
    Guid Id,
    Guid CustomerId,
    OrderStatus Status,
    decimal TotalPrice,
    DateTime CreatedAtUtc)
{
    public List<OrderItemResponse> OrderItems { get; } = [];
}
public sealed record OrderItemResponse(
    Guid OrderItemId,
    Guid OrderId,
    Guid TicketTypeId,
    decimal Quantity,
    decimal UnitPrice,
    decimal Price,
    string Currency);


internal sealed class GetOrderQueryHandler(IDbConnectionFactory dbConnectionFactory)
    : IQueryHandler<GetOrderQuery, OrderResponse>
{
    public async Task<Result<OrderResponse>> Handle(GetOrderQuery query, CancellationToken cancellationToken)
    {
        await using DbConnection connection = await dbConnectionFactory.OpenConnectionAsync();

        string sql =
            $"""
            SELECT
                o.id AS {nameof(OrderResponse.Id)},
                o.customer_id AS {nameof(OrderResponse.CustomerId)},
                o.status AS {nameof(OrderResponse.Status)},
                o.total_price AS {nameof(OrderResponse.TotalPrice)},
                o.created_at_utc AS {nameof(OrderResponse.CreatedAtUtc)},
                oi.id AS {nameof(OrderItemResponse.OrderItemId)},
                oi.order_id AS {nameof(OrderItemResponse.OrderId)},
                oi.ticket_type_id AS {nameof(OrderItemResponse.TicketTypeId)},
                oi.quantity AS {nameof(OrderItemResponse.Quantity)},
                oi.unit_price AS {nameof(OrderItemResponse.UnitPrice)},
                oi.price AS {nameof(OrderItemResponse.Price)},
                oi.currency AS {nameof(OrderItemResponse.Currency)}
            FROM ticketing.orders o
            LEFT JOIN ticketing.order_items oi ON oi.order_id = o.id
            WHERE o.id = @OrderId
            """;

        Dictionary<Guid, OrderResponse> ordersDictionary = [];

        await connection.QueryAsync<OrderResponse, OrderItemResponse, OrderResponse>(
            sql,
            (order, orderItem) =>
            {
                if (ordersDictionary.TryGetValue(order.Id, out OrderResponse? existingEvent))
                {
                    order = existingEvent;
                }
                else
                {
                    ordersDictionary.Add(order.Id, order);
                }

                order.OrderItems.Add(orderItem);

                return order;
            },
            query,
            splitOn: nameof(OrderItemResponse.OrderItemId));

        if (!ordersDictionary.TryGetValue(query.OrderId, out OrderResponse orderResponse))
        {
            return Result.Failure<OrderResponse>(OrderErrors.NotFound(query.OrderId));
        }

        return orderResponse;


    }
}
