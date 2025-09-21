using System.Data.Common;
using Bookings.Common.Application.Data;
using Bookings.Common.Application.Messaging;
using Bookings.Common.Domain;
using Bookings.Modules.Ticketing.Domain.Tickets;
using Dapper;

namespace Bookings.Modules.Ticketing.Application.Tickets.GetTicket;

internal sealed class GetTicketQueryHandler(
    IDbConnectionFactory dbConnectionFactory)
    : IQueryHandler<GetTicketQuery, TicketResponse>
{
    public async Task<Result<TicketResponse>> Handle(GetTicketQuery query, CancellationToken cancellationToken)
    {
        await using DbConnection connection = await dbConnectionFactory.OpenConnectionAsync();

        string sql =
            $"""
            SELECT
                id AS {nameof(TicketResponse.Id)},
                customer_id AS {nameof(TicketResponse.CustomerId)},
                order_id AS {nameof(TicketResponse.OrderId)},
                ticket_type_id AS {nameof(TicketResponse.TicketTypeId)},
                code AS {nameof(TicketResponse.Code)},
                created_at_utc AS {nameof(TicketResponse.CreatedAtUtc)}
            FROM ticketing.ticket
            WHERE id = @TicketId
            """;

        TicketResponse? ticket = await connection.QuerySingleOrDefaultAsync<TicketResponse>(sql, query.TicketId);

        if (ticket is null)
        {
            return Result.Failure<TicketResponse>(TicketErrors.NotFound(query.TicketId));
        }

        return ticket;

    }
}
