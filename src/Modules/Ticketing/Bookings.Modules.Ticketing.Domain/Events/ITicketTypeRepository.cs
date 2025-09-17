namespace Bookings.Modules.Ticketing.Domain.Events;

public interface ITicketTypeRepository
{
    Task<TicketType?> GetAsync(Guid id, CancellationToken cancellationToken);
    Task<TicketType?> GetWithLockAsync(Guid id, CancellationToken cancellationToken = default);
    void InsertRange(IEnumerable<TicketType> ticketTypes);
}
