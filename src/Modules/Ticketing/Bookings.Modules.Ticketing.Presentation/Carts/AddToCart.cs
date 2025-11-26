using Bookings.Common.Domain;
using Bookings.Common.Presentation.ApiResults;
using Bookings.Common.Presentation.Endpoints;
using Bookings.Modules.Ticketing.Application.Carts.AddItemToCart;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Bookings.Modules.Ticketing.Presentation.Carts;

internal sealed class AddToCart : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("cart/add", async (Request request, ISender sender) =>
        {
            Result result = await sender.Send(new AddItemToCartCommand(request.CustomerId, request.TicketTypeId, request.Quantity));

            return result.Match(() => Results.Ok(), ApiResults.Problem);
        })
        .RequireAuthorization()
        .WithTags(Tags.Carts);
    }
}

internal sealed class Request
{
    public Guid CustomerId { get; init; }
    public Guid TicketTypeId { get; init; }
    public decimal Quantity { get; init; }
}
