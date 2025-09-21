using Bookings.Common.Application.Messaging;

namespace Bookings.Modules.Ticketing.Application.Payments.RefundPaymentsForEvent;

public sealed record RefundPaymentsForEventCommand(Guid EventId) : ICommand;
