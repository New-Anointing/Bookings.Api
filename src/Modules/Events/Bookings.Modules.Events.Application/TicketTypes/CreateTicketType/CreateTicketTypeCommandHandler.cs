using Bookings.Modules.Events.Application.Abstractions.Data;
using Bookings.Modules.Events.Application.Abstractions.Messaging;
using Bookings.Modules.Events.Domain.Abstractions;
using Bookings.Modules.Events.Domain.Events;
using Bookings.Modules.Events.Domain.TicketTypes;

namespace Bookings.Modules.Events.Application.TicketTypes.CreateTicketType;


internal sealed class CreateTicketTypeCommandHandler(
    IUnitOfWork unitOfWork,
    ITicketTypeRepository ticketTypeRepository,
    IEventRepository eventRepository)
    : ICommandHandler<CreateTicketTypeCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateTicketTypeCommand command, CancellationToken cancellationToken)
    {
        Event? @event = await eventRepository.GetAsync(command.EventId, cancellationToken);
        if ( @event is null)
        {
            return Result.Failure<Guid>(EventErrors.NotFound(command.EventId));
        }

        var ticketType = TicketType.Create(@event, command.Name, command.Price, command.Currency, command.Quantity);

        ticketTypeRepository.Insert(ticketType);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return ticketType.Id;

    }
}
