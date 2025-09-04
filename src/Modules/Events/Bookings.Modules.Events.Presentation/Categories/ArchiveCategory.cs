using Bookings.Common.Domain;
using Bookings.Common.Presentation.ApiResults;
using Bookings.Common.Presentation.Endpoints;
using Bookings.Modules.Events.Application.Categories.ArchiveCategory;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Bookings.Modules.Events.Presentation.Categories;
internal sealed class ArchiveCategory : IEndpoint
{
    public void MapEndpoint (IEndpointRouteBuilder app)
    {
        app.MapPut("categories/{id}/archive", async (Guid id, ISender sender) =>
        {
            Result result = await sender.Send(new ArchiveCategoryCommand(id));

            result.Match(() => Results.Ok(), ApiResults.Problem);
        })
        .WithTags(Tags.Categories);
    }
}
