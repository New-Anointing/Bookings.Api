using Microsoft.AspNetCore.Routing;

namespace Bookings.Modules.Events.Presentation.Events;
public static class EventEndpoints
{
    public static void MapEndpoints(IEndpointRouteBuilder app)
    {
        CreateEvent.MapEndpoint(app);
        GetEvent.MapEndpoint(app);
        GetEvents.MapEndpoint(app);
        PublishEvent.MapEndpoint(app);
        CancelEvent.MapEndpoint(app);
        SearchEvents.MapEndpoint(app);
        RescheduledEvent.MapEndpoint(app);
    }
}
