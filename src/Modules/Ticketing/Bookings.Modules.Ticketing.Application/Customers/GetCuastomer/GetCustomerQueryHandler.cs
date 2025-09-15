using System.Data.Common;
using Bookings.Common.Application.Data;
using Bookings.Common.Application.Messaging;
using Bookings.Common.Domain;
using Bookings.Modules.Ticketing.Domain.Customers;
using Dapper;

namespace Bookings.Modules.Ticketing.Application.Customers.GetCuastomer;


internal sealed class GetCustomerQueryHandler(IDbConnectionFactory dbConnectionFactory)
    : IQueryHandler<GetCustomerQuery, CustomerResponse>
{
    public async Task<Result<CustomerResponse>> Handle(GetCustomerQuery query, CancellationToken cancellationToken)
    {
        await using DbConnection connection = await dbConnectionFactory.OpenConnectionAsync();

        string sql =
            $"""
            SEELCT
                id AS {nameof(CustomerResponse.Id)},
                email AS {nameof(CustomerResponse.Email)},
                first_name AS {nameof(CustomerResponse.FirstName)},
                last_name AS {nameof(CustomerResponse.LastName)}
            FROM customers.customers
            WHERE id = @CustomerId
            """;

        CustomerResponse? customer = await connection.QuerySingleOrDefaultAsync<CustomerResponse?>(sql, query);

        if (customer is null)
        {
            return Result.Failure<CustomerResponse>(CustomerErrors.NotFound(query.CustomerId));
        }

        return customer;
    }
}
