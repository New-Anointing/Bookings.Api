using Bookings.Common.Application.Clock;
using Bookings.Common.Application.Messaging;
using Bookings.Common.Domain;
using Bookings.Modules.Events.Application.Abstractions.Data;
using Bookings.Modules.Events.Domain.Events;

namespace Bookings.Modules.Events.Application.Events.RescheduleEvent;

internal sealed class RescheduleEventCommandHandler(
    IUnitOfWork unitOfWork,
    IEventRepository eventRepository,
    IDateTimeProvider dateTimeProvider)
    : ICommandHandler<RescheduleEventCommand>
{
    public async Task<Result> Handle(RescheduleEventCommand command, CancellationToken cancellationToken)
    {
        Event? @event = await eventRepository.GetAsync(command.EventId, cancellationToken);

        if (@event is null)
        {
            return Result.Failure(EventErrors.NotFound(command.EventId));
        }

        if (command.NewStartsAtUtc < dateTimeProvider.UtcNow)
        {
            return Result.Failure(EventErrors.StartDateInPast);
        }

        @event.Reschedule(command.NewStartsAtUtc, command.NewEndsAtUtc);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
