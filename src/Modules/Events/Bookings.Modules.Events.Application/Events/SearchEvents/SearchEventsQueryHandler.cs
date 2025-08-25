using System.Data.Common;
using Bookings.Modules.Events.Application.Abstractions.Data;
using Bookings.Modules.Events.Application.Abstractions.Messaging;
using Bookings.Modules.Events.Application.Events.GetEvents;
using Bookings.Modules.Events.Domain.Abstractions;
using Bookings.Modules.Events.Domain.Events;
using Dapper;

namespace Bookings.Modules.Events.Application.Events.SearchEvents;

internal sealed class SearchEventsQueryHandler(IDbConnectionFactory dbConnectionFactory)
    : IQueryHandler<SearchEventsQuery, SearchEventsResponse>
{
    public async Task<Result<SearchEventsResponse>> Handle(
        SearchEventsQuery query,
        CancellationToken cancellationToken)
    {
        await using DbConnection connection = await dbConnectionFactory.OpenConnectionAsync();

        var parameters = new SearchEventsParameters(
            (int)EventStatus.Published,
            query.CategoryId,
            query.StartDate?.Date,
            query.EndDate?.Date,
            query.PageSize,
            (query.Page - 1) * query.PageSize);

        IReadOnlyCollection<EventResponse> events = await GetEventsAsync(connection, parameters);

        int totalCount = await CountEventsAsync(connection, parameters);

        return new SearchEventsResponse(query.Page, query.PageSize, totalCount, events);
    }

    private async Task<IReadOnlyCollection<EventResponse>> 
        GetEventsAsync(DbConnection connection, SearchEventsParameters parameters)
    {
        const string sql =
            $"""
             SELECT
                 id AS {nameof(EventResponse.Id)},
                 category_id AS {nameof(EventResponse.CategoryId)},
                 title AS {nameof(EventResponse.Title)},
                 description AS {nameof(EventResponse.Description)},
                 location AS {nameof(EventResponse.Location)},
                 starts_at_utc AS {nameof(EventResponse.StartsAtUtc)},
                 ends_at_utc AS {nameof(EventResponse.EndsAtUtc)}
             FROM events.events
             WHERE
                status = @Status AND
                (@CategoryId IS NULL OR category_id = @CategoryId) AND
                (@StartDate::timestamp IS NULL OR starts_at_utc >= @StartDate::timestamp) AND
                (@EndDate::timestamp IS NULL OR ends_at_utc >= @EndDate::timestamp)
             ORDER BY starts_at_utc
             OFFSET @Skip
             LIMIT @Take
             """;

        List<EventResponse> events = (await connection.QueryAsync<EventResponse>(sql, parameters)).AsList();

        return events;
    }

    private static async Task<int> CountEventsAsync(DbConnection connection, SearchEventsParameters parameters)
    {
        const string sql =
            """
            SELECT COUNT(*)
            FROM events.events
            WHERE
               status = @Status AND
               (@CategoryId IS NULL OR category_id = @CategoryId) AND
               (@StartDate::timestamp IS NULL OR starts_at_utc >= @StartDate::timestamp) AND
               (@EndDate::timestamp IS NULL OR ends_at_utc >= @EndDate::timestamp)
            """;

        int totalCount = await connection.ExecuteScalarAsync<int>(sql, parameters);

        return totalCount;
    }


    private sealed record SearchEventsParameters(
           int Status,
           Guid? CategoryId,
           DateTime? StartDate,
           DateTime? EndDate,
           int Take,
           int Skip);
}

