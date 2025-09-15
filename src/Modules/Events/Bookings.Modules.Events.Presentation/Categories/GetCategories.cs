using Bookings.Common.Application.Caching;
using Bookings.Common.Domain;
using Bookings.Common.Presentation.ApiResults;
using Bookings.Common.Presentation.Endpoints;
using Bookings.Modules.Events.Application.Categories.GetCategories;
using Bookings.Modules.Events.Application.Categories.GetCategory;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Bookings.Modules.Events.Presentation.Categories;

internal sealed class GetCategories : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("categories", async (ISender sender, ICacheService cacheService) =>
        {
            IReadOnlyCollection<CategoryResponse> categoryResponse =
                await cacheService.GetAsync<IReadOnlyCollection<CategoryResponse>>("categories");

            if (categoryResponse is not null)
            {
                return Results.Ok(categoryResponse);
            }

            Result<IReadOnlyCollection<CategoryResponse>> result = await sender.Send(new GetCategoriesQuery());

            if (result.IsSuccess)
            {
                await cacheService.SetAsync("categories", result.Value);
            }

            return result.Match(Results.Ok, ApiResults.Problem);
        })
        .WithTags(Tags.Categories);
    }
}
