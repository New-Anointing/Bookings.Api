using MediatR;

namespace Bookings.Common.Domain;

public interface IDomainEvent : INotification
{
    Guid Id { get; }
    DateTime OccuredOnUtc { get; }
}
