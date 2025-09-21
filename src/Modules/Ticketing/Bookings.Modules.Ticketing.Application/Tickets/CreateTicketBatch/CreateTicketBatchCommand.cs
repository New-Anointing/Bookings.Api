using Bookings.Common.Application.Messaging;

namespace Bookings.Modules.Ticketing.Application.Tickets.CreateTicketBatch;

public sealed record CreateTicketBatchCommand(Guid OrderId) : ICommand;
