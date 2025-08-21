using System.Data.Common;
using Bookings.Modules.Events.Application.Abstractions.Data;
using Bookings.Modules.Events.Application.Abstractions.Messaging;
using Bookings.Modules.Events.Domain.Abstractions;
using Bookings.Modules.Events.Domain.Events;
using Dapper;
using MediatR;

namespace Bookings.Modules.Events.Application.Events.GetEvent;

internal sealed class GetEventQueryHandler(IDbConnectionFactory dbCconnectionFactory) : IQueryHandler<GetEventQuery, EventResponse>
{

    public async Task<Result<EventResponse>> Handle
        (GetEventQuery query, CancellationToken cancellationToken)
    {
        await using DbConnection connection = await dbCconnectionFactory.OpenConnectionAsync();

        const string sql =
            $"""
            SELECT
                e.id AS {nameof(EventResponse.Id)},
                e.category_id AS {nameof(EventResponse.CategoryId)},
                e.title AS {nameof(EventResponse.Title)},
                description AS {nameof(EventResponse.Description)}.,
                e.location AS {nameof(EventResponse.Location)},
                e.starts_at_utc AS {nameof(EventResponse.StartsAtUtc)},
                e.ends_at_utc AS {nameof(EventResponse.EndsAtUtc)},
                tt.id AS {nameof(TicketTypeResponse.TicketTypeId)},
                tt.name AS {nameof(TicketTypeResponse.Name)},
                tt.price AS {nameof(TicketTypeResponse.Price)},
                tt.currency AS {nameof(TicketTypeResponse.Currency)},
                tt.quantity AS {nameof(TicketTypeResponse.Quantity)}
            FROM events.events e
            LEFT JOIN events.ticket_types tt ON tt.event_id = e.id
            WHERE id = @EventId
            """;

        Dictionary<Guid, EventResponse> eventsDictionary = [];
        await connection.QueryAsync<EventResponse, TicketTypeResponse?, EventResponse>(
            sql,
            (@event, ticketType) =>
            {
                if (eventsDictionary.TryGetValue(@event.Id, out EventResponse? existingEvent))
                {
                    @event = existingEvent;
                }
                else
                {
                    eventsDictionary.Add(@event.Id, @event);
                }

                if (ticketType is not null)
                {
                    @event.TicketTypes.Add(ticketType);
                }

                return @event;
            },
            query,
            splitOn: nameof(TicketTypeResponse.TicketTypeId));

        if (!eventsDictionary.TryGetValue(query.EventId, out EventResponse eventResponse))
        {
            return Result.Failure<EventResponse>(EventErrors.NotFound(query.EventId));
        }

        return eventResponse;
    }
}
