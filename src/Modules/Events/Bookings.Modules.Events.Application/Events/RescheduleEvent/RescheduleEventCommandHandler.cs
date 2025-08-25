using Bookings.Modules.Events.Application.Abstractions.Clock;
using Bookings.Modules.Events.Application.Abstractions.Data;
using Bookings.Modules.Events.Application.Abstractions.Messaging;
using Bookings.Modules.Events.Domain.Abstractions;
using Bookings.Modules.Events.Domain.Events;
using MediatR;

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

        if(@event is null)
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
