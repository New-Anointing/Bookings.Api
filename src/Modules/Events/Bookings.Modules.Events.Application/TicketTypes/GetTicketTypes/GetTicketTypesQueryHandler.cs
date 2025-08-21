using System.Data;
using System.Data.Common;
using Bookings.Modules.Events.Application.Abstractions.Data;
using Bookings.Modules.Events.Application.Abstractions.Messaging;
using Bookings.Modules.Events.Application.TicketTypes.GetTicketType;
using Bookings.Modules.Events.Domain.Abstractions;
using Dapper;


namespace Bookings.Modules.Events.Application.TicketTypes.GetTicketTypes;

internal sealed class GetTicketTypesQueryHandler(IDbConnectionFactory dbConnectionFactory)
    : IQueryHandler<GetTicketTypesQuery, IReadOnlyCollection<TicketTypeResponse>>
{
    public async Task<Result<IReadOnlyCollection<TicketTypeResponse>>> Handle(GetTicketTypesQuery query, CancellationToken cancellationToken)
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
            WHERE event_id = @EventId
            """;

        List<TicketTypeResponse> ticketTypes = (await connection.QueryAsync<TicketTypeResponse>(sql, query)).AsList();

        return ticketTypes;
    }
}
