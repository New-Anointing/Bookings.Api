namespace Bookings.Modules.Events.Domain.TicketTypes;

public interface ITicketTypeRepository
{
    Task<TicketType?> GetAsync(Guid id, CancellationToken cancellationToken);
    Task<bool> ExistAsync(Guid eventId, CancellationToken cancellationToken);
    void Insert(TicketType ticketType);
}
