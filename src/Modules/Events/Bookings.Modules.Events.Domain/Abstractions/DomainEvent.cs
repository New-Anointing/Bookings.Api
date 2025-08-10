namespace Bookings.Modules.Events.Domain.Abstractions;

public abstract class DomainEvent : IDomainEvent
{
    protected DomainEvent() 
    {
        Id = Guid.NewGuid();
        OccuredOnUtc = DateTime.UtcNow;
    }

    protected DomainEvent(Guid id, DateTime occuredAtUtc)
    {
        Id = id;
        OccuredAtUtc = occuredAtUtc;
    }

    public Guid Id { get; init; }
    public DateTime OccuredAtUtc { get; }
    public DateTime OccuredOnUtc { get; init; }
}
