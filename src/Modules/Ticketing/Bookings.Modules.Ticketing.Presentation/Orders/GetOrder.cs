using Bookings.Common.Domain;
using Bookings.Common.Presentation.ApiResults;
using Bookings.Common.Presentation.Endpoints;
using Bookings.Modules.Ticketing.Application.Orders.GetOrder;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Bookings.Modules.Ticketing.Presentation.Orders;

internal sealed class GetOrder : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("orders/{id}", async (ISender sender, Guid id) =>
        {
            Result<OrderResponse> result = await sender.Send(new GetOrderQuery(id));

            return result.Match(Results.Ok, ApiResults.Problem);
        })
        .WithTags(Tags.Orders);
    }
}
