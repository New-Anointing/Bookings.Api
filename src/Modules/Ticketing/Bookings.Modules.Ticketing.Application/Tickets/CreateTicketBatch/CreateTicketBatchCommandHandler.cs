using System.Data.Common;
using Bookings.Common.Application.Messaging;
using Bookings.Common.Domain;
using Bookings.Modules.Ticketing.Application.Abstractions.Data;
using Bookings.Modules.Ticketing.Domain.Events;
using Bookings.Modules.Ticketing.Domain.Orders;
using Bookings.Modules.Ticketing.Domain.Tickets;

namespace Bookings.Modules.Ticketing.Application.Tickets.CreateTicketBatch;

internal sealed class CreateTicketBatchCommandHandler(
    IUnitOfWork unitOfWork,
    IOrderRepository orderRepository,
    ITicketTypeRepository ticketTypeRepository,
    ITicketRepository ticketRepository)
    : ICommandHandler<CreateTicketBatchCommand>
{
    public async Task<Result> Handle(CreateTicketBatchCommand command, CancellationToken cancellationToken)
    {
        await using DbTransaction transaction = await unitOfWork.BeginTransactionAsync(cancellationToken);

        Order? order = await orderRepository.GetAsync(command.OrderId, cancellationToken);

        if (order is null)
        {
            return Result.Failure(OrderErrors.NotFound(command.OrderId));
        }

        Result? result = order.IssueTickets();

        if (result.IsFailure)
        {
            return Result.Failure(result.Error);
        }

        List<Ticket> tickets = [];

        foreach (OrderItem orderItem in order.OrderItems)
        {
            TicketType? ticketType = await ticketTypeRepository.GetAsync(orderItem.TicketTypeId, cancellationToken);

            if (ticketType is null)
            {
                return Result.Failure(TicketTypeErrors.NotFound(orderItem.TicketTypeId));
            }

            for (int i = 0; i < orderItem.Quantity; i++)
            {
                var ticket = Ticket.Create(order, ticketType);

                tickets.Add(ticket);
            }
        }


        ticketRepository.InsertRange(tickets);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        await transaction.CommitAsync(cancellationToken);

        return Result.Success();
    }
}
