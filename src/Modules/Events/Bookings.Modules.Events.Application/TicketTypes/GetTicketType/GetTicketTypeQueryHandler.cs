using System.Data.Common;
using Bookings.Modules.Events.Application.Abstractions.Data;
using Bookings.Modules.Events.Application.Abstractions.Messaging;
using Bookings.Modules.Events.Domain.Abstractions;
using Bookings.Modules.Events.Domain.TicketTypes;
using Dapper;

namespace Bookings.Modules.Events.Application.TicketTypes.GetTicketType;

internal sealed class GetTicketTypeQueryHandler
    (IDbConnectionFactory dbConnectionFactory) 
    : IQueryHandler<GetTicketTypeQuery, TicketTypeResponse>
{
    public async Task<Result<TicketTypeResponse>> Handle(GetTicketTypeQuery query, CancellationToken cancellationToken)
    {
        await using DbConnection connection = await dbConnectionFactory.OpenConnectionAsync();

        const string sql =
             $"""
             SELECT
                 id AS {nameof(TicketTypeResponse.Id)},
                 event_id AS {nameof(TicketTypeResponse.EventId)},
                 name AS {nameof(TicketTypeResponse.Name)},
                 price AS {nameof(TicketTypeResponse.Price)},
                 currency AS {nameof(TicketTypeResponse.Currency)},
                 quantity AS {nameof(TicketTypeResponse.Quantity)}
             FROM events.ticket_types
             WHERE id = @TicketTypeId
             """;
        TicketTypeResponse? ticketType =
           await connection.QuerySingleOrDefaultAsync<TicketTypeResponse>(sql, query);

        if (ticketType is null)
        {
            return Result.Failure<TicketTypeResponse>(TicketTypeErrors.NotFound(query.TicketTypeId));
        }

        return ticketType;

    }
}
