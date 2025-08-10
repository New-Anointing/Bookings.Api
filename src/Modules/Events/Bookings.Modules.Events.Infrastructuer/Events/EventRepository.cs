using Bookings.Modules.Events.Domain.Events;
using Bookings.Modules.Events.Infrastructure.Database;

namespace Bookings.Modules.Events.Infrastructure.Events;
internal sealed class EventRepository(EventsDbContext context) : IEventRepository
{
    public void Insert(Event @event)
    {
        context.Events.Add(@event);
    }
}
