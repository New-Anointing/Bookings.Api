using Bookings.Modules.Events.Domain.Events;
using Bookings.Modules.Events.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Bookings.Modules.Events.Infrastructure.Events;
internal sealed class EventRepository(EventsDbContext context) : IEventRepository
{
    public async Task<Event?> GetAsync(Guid id, CancellationToken cancellationToken)
    {
        return await context.Events.Where(e => e.Id == id).SingleOrDefaultAsync(cancellationToken);
    }

    public void Insert(Event @event)
    {
        context.Events.Add(@event);
    }
}
