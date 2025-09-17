using Bookings.Common.Application.Messaging;
using Bookings.Common.Domain;
using Bookings.Modules.Ticketing.Application.Abstractions.Data;
using Bookings.Modules.Ticketing.Domain.Events;

namespace Bookings.Modules.Ticketing.Application.Events.CancelEvent;

internal sealed class CancelEventCommandHandler(IEventRepository eventRepository, IUnitOfWork unitOfWork) : ICommandHandler<CancelEventCommand>
{
    public async Task<Result> Handle(CancelEventCommand command, CancellationToken cancellationToken)
    {
        Event? @event = await eventRepository.GetAsync(command.EventId, cancellationToken);

        if (@event is null)
        {
            return Result.Failure(EventErrors.NotFound(command.EventId));
        }

        @event.Cancel();

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
