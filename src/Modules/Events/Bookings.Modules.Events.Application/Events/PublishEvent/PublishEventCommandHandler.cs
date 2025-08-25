using Bookings.Modules.Events.Application.Abstractions.Data;
using Bookings.Modules.Events.Application.Abstractions.Messaging;
using Bookings.Modules.Events.Domain.Abstractions;
using Bookings.Modules.Events.Domain.Events;
using Bookings.Modules.Events.Domain.TicketTypes;

namespace Bookings.Modules.Events.Application.Events.PublishEvent;

internal sealed class PublishEventCommandHandler(
    IUnitOfWork unitOfWork,
    IEventRepository eventRepository,
    ITicketTypeRepository ticketTypeRepository) 
    : ICommandHandler<PublishEventCommand>
{
    public async Task<Result> Handle(PublishEventCommand command, CancellationToken cancellationToken)
    {
        Event? @event = await eventRepository.GetAsync(command.EventId, cancellationToken);

        if (@event is null)
        {
            return Result.Failure(EventErrors.NotFound(command.EventId));
        }

        if(!await ticketTypeRepository.ExistAsync(@event.Id, cancellationToken))
        {
            return Result.Failure(EventErrors.NoTicketsFound);
        }

        Result result = @event.Publish();

        if(result.IsFailure)
        {
            return Result.Failure(result.Error);
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
