using Bookings.Common.Application.Messaging;

namespace Bookings.Modules.Ticketing.Application.Tickets.ArchiveTicketsFrorEvent;

public sealed record ArchiveTicketsForEventCommand(Guid EventId) : ICommand;
