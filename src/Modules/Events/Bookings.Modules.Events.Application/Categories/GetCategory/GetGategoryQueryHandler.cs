using System.Data.Common;
using Bookings.Common.Application.Data;
using Bookings.Common.Application.Messaging;
using Bookings.Common.Domain;
using Bookings.Modules.Events.Domain.Categories;
using Dapper;

namespace Bookings.Modules.Events.Application.Categories.GetCategory;

internal sealed class GetGategoryQueryHandler(IDbConnectionFactory dbConnectionFactory)
    : IQueryHandler<GetCategoryQuery, CategoryResponse>
{
    public async Task<Result<CategoryResponse>> Handle(GetCategoryQuery query, CancellationToken cancellationToken)
    {
        await using DbConnection connection = await dbConnectionFactory.OpenConnectionAsync();

        const string sql =
            $"""
            SELECT
                id AS {nameof(CategoryResponse.Id)},
                name AS {nameof(CategoryResponse.Name)},
                is_archived AS {nameof(CategoryResponse.IsArchived)}
            FROM events.categories
            WHERE id = @CategoryId
            """;

        CategoryResponse? category = await connection.QuerySingleOrDefaultAsync<CategoryResponse?>(sql, query);

        if (category is null)
        {
            return Result.Failure<CategoryResponse>(CategoryErrors.NotFound(query.CategoryId));
        }

        return category;
    }
}


