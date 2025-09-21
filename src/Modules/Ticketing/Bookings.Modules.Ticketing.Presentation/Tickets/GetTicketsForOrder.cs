using Bookings.Common.Domain;
using Bookings.Common.Presentation.ApiResults;
using Bookings.Common.Presentation.Endpoints;
using Bookings.Modules.Ticketing.Application.Tickets.GetTicket;
using Bookings.Modules.Ticketing.Application.Tickets.GetTicketForOrder;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Bookings.Modules.Ticketing.Presentation.Tickets;

internal sealed class GetTicketForOrderQuery : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("ticket/order/{orderId}", async (ISender sender, Guid orderId) =>
        {
            Result<IReadOnlyCollection<TicketResponse>> result = await sender.Send(new GetTicketsForOrderQuery(orderId));

            return result.Match(Results.Ok, ApiResults.Problem);
        })
        .WithTags(Tags.Tickets);
    }
}
