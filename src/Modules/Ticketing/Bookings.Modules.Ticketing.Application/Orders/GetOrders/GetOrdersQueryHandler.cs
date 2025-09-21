using System.Data.Common;
using Bookings.Common.Application.Data;
using Bookings.Common.Application.Messaging;
using Bookings.Common.Domain;
using Dapper;

namespace Bookings.Modules.Ticketing.Application.Orders.GetOrders;

internal sealed class GetOrdersQueryHandler(IDbConnectionFactory dbConnectionFactory)
    : IQueryHandler<GetOrdersQuery, IReadOnlyCollection<OrderResponse>>
{
    public async Task<Result<IReadOnlyCollection<OrderResponse>>> Handle(GetOrdersQuery query, CancellationToken cancellationToken)
    {
        await using DbConnection connection = await dbConnectionFactory.OpenConnectionAsync();

        string sql =
            $"""
            SELECT
                id AS {nameof(OrderResponse.Id)},
                customer_id AS {nameof(OrderResponse.CustomerId)},
                order_status AS {nameof(OrderResponse.Status)},
                total_price AS {nameof(OrderResponse.TotalPrice)},
                created_at AS {nameof(OrderResponse.CreatedAtUtc)},
            FROM ticketing.orders
            WHERE customer_id = @CustomerId
            """;

        List<OrderResponse> orders = (await connection.QueryAsync<OrderResponse>(sql, query)).AsList();

        return orders;
    }
}
