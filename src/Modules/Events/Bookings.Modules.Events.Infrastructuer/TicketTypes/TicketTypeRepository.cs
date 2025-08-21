using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bookings.Modules.Events.Domain.TicketTypes;
using Bookings.Modules.Events.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Bookings.Modules.Events.Infrastructure.TicketTypes;
public class TicketTypeRepository(EventsDbContext context) : ITicketTypeRepository
{
    public Task<TicketType?> GetAsync(Guid id, CancellationToken cancellationToken)
    {
        return context.TicketTypes.Where(c => c.Id == id).SingleOrDefaultAsync(cancellationToken);
    }

    public void Insert(TicketType ticketType)
    {
        context.TicketTypes.Add(ticketType);
    }
}
