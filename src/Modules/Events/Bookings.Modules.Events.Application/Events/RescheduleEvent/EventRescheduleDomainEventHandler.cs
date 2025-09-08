using Bookings.Common.Application.Messaging;
using Bookings.Modules.Events.Domain.Events;

namespace Bookings.Modules.Events.Application.Events.RescheduleEvent;
internal sealed class RescheduleDomainEventHandler : IDomainEventHandler<EventRescheduledDomainEvent>
{
    public Task Handle(EventRescheduledDomainEvent notification, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
