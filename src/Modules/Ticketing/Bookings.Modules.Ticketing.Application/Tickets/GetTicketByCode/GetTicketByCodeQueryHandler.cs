using System.Data.Common;
using Bookings.Common.Application.Data;
using Bookings.Common.Application.Messaging;
using Bookings.Common.Domain;
using Bookings.Modules.Ticketing.Application.Tickets.GetTicket;
using Bookings.Modules.Ticketing.Domain.Tickets;
using Dapper;

namespace Bookings.Modules.Ticketing.Application.Tickets.GetTicketByCode;

internal sealed class GetTicketByCodeQueryHandler(IDbConnectionFactory dbConnectionFactory)
    : IQueryHandler<GetTicketByCodeQuery, TicketResponse>
{
    public async Task<Result<TicketResponse>> Handle(GetTicketByCodeQuery query, CancellationToken cancellationToken)
    {
        await using DbConnection connection = await dbConnectionFactory.OpenConnectionAsync();

        string sql =
            $"""
            SELECT
                id AS {nameof(TicketResponse.Id)},
                customer_id AS {nameof(TicketResponse.CustomerId)},
                order_id AS {nameof(TicketResponse.OrderId)},
                event_id AS {nameof(TicketResponse.EventId)},
                ticket_type_id {nameof(TicketResponse.TicketTypeId)},
                code AS {nameof(TicketResponse.Code)},
                created_at_utc AS {nameof(TicketResponse.CreatedAtUtc)}
            FROM ticketing.tickets
            WHERE code = @Code
            """;

        TicketResponse? ticket = await connection.QuerySingleOrDefaultAsync<TicketResponse>(sql, query.Code);

        if (ticket is null)
        {
            return Result.Failure<TicketResponse>(TicketErrors.NotFound(query.Code));
        }

        return ticket;
    }
}
