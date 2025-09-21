using Bookings.Common.Application.Messaging;

namespace Bookings.Modules.Ticketing.Application.Payments.RefundPayment;

public sealed record RefundPaymentCommand(Guid PaymentId, decimal Amount) : ICommand;
