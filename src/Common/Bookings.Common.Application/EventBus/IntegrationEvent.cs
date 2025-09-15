namespace Bookings.Common.Application.EventBus;

public abstract class IntegrationEvent : IIntegrationEvent
{
    public Guid Id { get; init; }
    public DateTime OccuredOnUtc { get; init; }
    protected IntegrationEvent(Guid id, DateTime occuredOnUtc)
    {
        Id = id;
        OccuredOnUtc = occuredOnUtc;
    }
}
