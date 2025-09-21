using System.Data.Common;
using Bookings.Common.Application.Messaging;
using Bookings.Common.Domain;
using Bookings.Modules.Ticketing.Application.Abstractions.Data;
using Bookings.Modules.Ticketing.Domain.Events;
using Bookings.Modules.Ticketing.Domain.Tickets;

namespace Bookings.Modules.Ticketing.Application.Tickets.ArchiveTicketsFrorEvent;

internal sealed class ArchiveTicketsForEventCommandHandler(
    ITicketRepository ticketRepository,
    IEventRepository eventRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<ArchiveTicketsForEventCommand>
{
    public async Task<Result> Handle(ArchiveTicketsForEventCommand command, CancellationToken cancellationToken)
    {
        await using DbTransaction transaction = await unitOfWork.BeginTransactionAsync(cancellationToken);

        Event? @event = await eventRepository.GetAsync(command.EventId, cancellationToken);

        if (@event is null)
        {
            return Result.Failure(EventErrors.NotFound(command.EventId));
        }

        IEnumerable<Ticket> tickets = await ticketRepository.GetForEventAsync(@event, cancellationToken);

        foreach (Ticket ticket in tickets)
        {
            ticket.Archive();
        }

        @event.TicketsArchived();

        await unitOfWork.SaveChangesAsync(cancellationToken);

        await transaction.CommitAsync(cancellationToken);

        return Result.Success();
    }
}
