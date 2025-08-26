using Bookings.Modules.Events.Application.Events.RescheduleEvent;
using Bookings.Modules.Events.Domain.Abstractions;
using Bookings.Modules.Events.Presentation.ApiResults;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Bookings.Modules.Events.Presentation.Events;
internal static class RescheduledEvent
{
    public static void MapEndpoint (IEndpointRouteBuilder app)
    {
        app.MapPut("events/{id}/reschedule", async (Guid id, Request request, ISender sender) =>
        {
            Result result = await sender.Send(new RescheduleEventCommand(id, request.StartsAtUtc, request.EndsAtUtc));

            return result.Match(Results.NoContent, ApiResults.ApiResults.Problem);
        })
        .WithTags(Tags.Events);
    }

    internal sealed class Request
    {
        public DateTime StartsAtUtc { get; init; }

        public DateTime? EndsAtUtc { get; init; }
    }
}
