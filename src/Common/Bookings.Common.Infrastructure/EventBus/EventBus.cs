using Bookings.Common.Application.EventBus;
using MassTransit;

namespace Bookings.Common.Infrastructure.EventBus;

internal sealed class EventBus(IBus bus) : IEventBus
{
    async Task IEventBus.PublishAsync<TEvent>(TEvent integrationEvent, CancellationToken cancellationToken)
    {
        await bus.Publish(integrationEvent, cancellationToken);
    }
}
