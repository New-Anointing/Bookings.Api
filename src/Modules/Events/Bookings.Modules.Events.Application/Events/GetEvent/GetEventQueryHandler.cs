using System.Data.Common;
using Bookings.Modules.Events.Application.Abstractions.Data;
using Bookings.Shared.Abstractions.CQRS;
using Dapper;

namespace Bookings.Modules.Events.Application.Events.GetEvent;

internal sealed class GetEventQueryHandler(IDbConnectionFactory dbCconnectionFactory) : IQueryHandler<GetEventQuery, EventResponse>
{
    public async Task<EventResponse> Handle(GetEventQuery request, CancellationToken cancellationToken)
    {
        await using DbConnection connection = await dbCconnectionFactory.OpenConnectionAssync();

        const string sql =
            $"""
            SELECT
                id AS {nameof(EventResponse.Id)},
                title AS {nameof(EventResponse.Title)},
                description AS {nameof(EventResponse.Description)}.,
                location AS {nameof(EventResponse.Location)},
                starts_at_utc AS {nameof(EventResponse.StartsAtUtc)},
                ends_at_utc AS {nameof(EventResponse.EndsAtUtc)}
            FROM events.events
            WHERE id = @EventId
            """;

        EventResponse @event = await connection.QuerySingleOrDefaultAsync(sql, request);
        return @event;
    }
}
