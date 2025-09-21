using Bookings.Common.Application.Messaging;
using Bookings.Common.Domain;
using Bookings.Modules.Ticketing.Application.Abstractions.Data;
using Bookings.Modules.Ticketing.Domain.Payment;

namespace Bookings.Modules.Ticketing.Application.Payments.RefundPayment;

internal sealed class RefundPaymentCommandHandler(
    IUnitOfWork unitOfWork,
    IPaymentRepository paymentRepository)
    : ICommandHandler<RefundPaymentCommand>
{
    public async Task<Result> Handle(RefundPaymentCommand command, CancellationToken cancellationToken)
    {
        Payment? payment = await paymentRepository.GetAsync(command.PaymentId, cancellationToken);

        if (payment is null)
        {
            return Result.Failure(PaymentErrors.NotFound(command.PaymentId));
        }

        Result result = payment.Refund(command.Amount);

        if (result.IsFailure)
        {
            return Result.Failure(result.Error);
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
