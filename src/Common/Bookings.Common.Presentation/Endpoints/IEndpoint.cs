using Microsoft.AspNetCore.Routing;

namespace Bookings.Common.Presentation.Endpoints;

public interface IEndpoint
{
    void MapEndpoint(IEndpointRouteBuilder app);
}
