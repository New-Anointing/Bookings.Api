using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookings.Modules.Events.Domain.TicketTypes;
public interface ITicketTypeRepository
{
    Task<TicketType?> GetAsync(Guid id, CancellationToken cancellationToken);
    Task<bool> ExistAsync(Guid eventId, CancellationToken cancellationToken);
    void Insert(TicketType ticketType);
}
