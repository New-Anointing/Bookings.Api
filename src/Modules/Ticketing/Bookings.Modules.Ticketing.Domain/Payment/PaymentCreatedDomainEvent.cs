using Bookings.Common.Domain;

namespace Bookings.Modules.Ticketing.Domain.Payment;

public sealed class PaymentCreatedDomainEvent(Guid paymentId) : DomainEvent
{
    public Guid PaymentId { get; init; } = paymentId;
}
