using Microsoft.AspNetCore.Routing;

namespace Bookings.Modules.Events.Presentation.Events;
public static class EventEndpoints
{
    public static void MapEndpoints(IEndpointRouteBuilder app)
    {
        CreateEvent.MapEndPoint(app);
        GetEvent.MapEndPoint(app);
    }
}
