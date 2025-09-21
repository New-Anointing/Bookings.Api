using System.Data.Common;
using Bookings.Common.Application.Messaging;
using Bookings.Common.Domain;
using Bookings.Modules.Ticketing.Application.Abstractions.Data;
using Bookings.Modules.Ticketing.Domain.Events;
using Bookings.Modules.Ticketing.Domain.Payment;

namespace Bookings.Modules.Ticketing.Application.Payments.RefundPaymentsForEvent;

internal sealed class RefundPaymentsForEventCommandHandler(
    IPaymentRepository paymentRepository,
    IEventRepository eventRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<RefundPaymentsForEventCommand>
{
    public async Task<Result> Handle(RefundPaymentsForEventCommand command, CancellationToken cancellationToken)
    {
        await using DbTransaction transaction = await unitOfWork.BeginTransactionAsync(cancellationToken);

        Event? @event = await eventRepository.GetAsync(command.EventId, cancellationToken);

        if (@event is null)
        {
            return Result.Failure(EventErrors.NotFound(command.EventId));
        }

        IEnumerable<Payment> payments = await paymentRepository.GetForEventAsync(@event, cancellationToken);

        foreach (Payment payment in payments)
        {
            payment.Refund(payment.Amount - (payment.AmountRefunded ?? decimal.Zero));
        }

        @event.PaymentsRefunded();

        await unitOfWork.SaveChangesAsync(cancellationToken);

        await transaction.CommitAsync(cancellationToken);

        return Result.Success();
    }
}
