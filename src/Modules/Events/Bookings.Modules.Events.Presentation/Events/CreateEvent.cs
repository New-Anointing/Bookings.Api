using Bookings.Modules.Events.Application.Events.CreateEvent;
using Bookings.Modules.Events.Domain.Abstractions;
using Bookings.Modules.Events.Presentation.ApiResults;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Bookings.Modules.Events.Presentation.Events;
internal static partial class CreateEvent
{
    public static void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/events", async (Request request, ISender sender) =>
        {
            var command = new CreateEventCommand(
                request.Title,
                request.CategoryId,
                request.Description,
                request.Location,
                request.StartsAtUtc,
                request.EndsAtUtc);

            Result<Guid> result = await sender.Send(command);

            return result.Match(Results.Ok, ApiResults.ApiResults.Problem);
        })
            .WithTags(Tags.Events);
    }

    internal sealed class Request
    {
        public string Title { get; set; }

        public Guid CategoryId { get; set; }

        public string Description { get; set; }

        public string Location { get; set; }

        public DateTime StartsAtUtc { get; set; }

        public DateTime? EndsAtUtc { get; set; }
    }

}
