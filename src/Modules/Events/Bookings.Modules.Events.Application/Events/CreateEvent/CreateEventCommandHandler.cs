using Bookings.Modules.Events.Application.Abstractions.Data;
using Bookings.Modules.Events.Domain.Events;
using Bookings.Shared.Abstractions.CQRS;

namespace Bookings.Modules.Events.Application.Events.CreateEvent;

internal sealed partial class CreateEventCommandHandler(IEventRepository eventRepository, IUnitOfWork unitOfWork) : 
    ICommandHandler<CreateEventCommand, Guid>
{

    public async Task<Guid> Handle(CreateEventCommand request, CancellationToken cancellationToken)
    {
        var @event = Event.Create(
            request.Title,
            request.Description,
            request.Location,
            request.StartsAtUtc,
            request.EndsAtUtc);

        eventRepository.Insert(@event);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return @event.Id;
    }

}
