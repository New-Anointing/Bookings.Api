using Bookings.Modules.Events.Application.Abstractions.Clock;
using Bookings.Modules.Events.Application.Abstractions.Data;
using Bookings.Modules.Events.Application.Abstractions.Messaging;
using Bookings.Modules.Events.Domain.Abstractions;
using Bookings.Modules.Events.Domain.Events;

namespace Bookings.Modules.Events.Application.Events.CancelEvent;

internal sealed class CancelEventCommandHandler
    (IEventRepository eventRepository,
    IUnitOfWork unitOfWork,
    IDateTimeProvider dateTimeProvider) 
    : ICommandHandler<CancelEventCommand>
{
    public async Task<Result> Handle(CancelEventCommand command, CancellationToken cancellationToken)
    {
        Event? @event = await eventRepository.GetAsync(command.EventId, cancellationToken);

        if(@event is null)
        {
            return Result.Failure(EventErrors.NotFound(command.EventId));
        }

        Result result = @event.Cancel(dateTimeProvider.UtcNow);

        if (result.IsFailure)
        {
            return Result.Failure(result.Error);
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
