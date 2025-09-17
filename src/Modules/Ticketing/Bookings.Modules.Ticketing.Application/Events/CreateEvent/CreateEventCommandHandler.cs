using Bookings.Common.Application.Messaging;
using Bookings.Common.Domain;
using Bookings.Modules.Ticketing.Application.Abstractions.Data;
using Bookings.Modules.Ticketing.Domain.Events;

namespace Bookings.Modules.Ticketing.Application.Events.CreateEvent;

internal sealed class CreateEventCommandHandler(
    IEventRepository eventRepository,
    ITicketTypeRepository ticketTypeRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<CreateEventCommand>
{
    public async Task<Result> Handle(CreateEventCommand command, CancellationToken cancellationToken)
    {
        var @event = Event.Create(
            command.EventId,
            command.Title,
            command.Description,
            command.Location,
            command.StartsAtUtc,
            command.EndsAtUtc);

        eventRepository.Insert(@event);

        IEnumerable<TicketType> ticketTypes = command.TicketTypes
            .Select(tt => TicketType.Create(tt.TicketTypeId, tt.EventId, tt.Name, tt.Price, tt.Currency, tt.Quantity));

        ticketTypeRepository.InsertRange(ticketTypes);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
