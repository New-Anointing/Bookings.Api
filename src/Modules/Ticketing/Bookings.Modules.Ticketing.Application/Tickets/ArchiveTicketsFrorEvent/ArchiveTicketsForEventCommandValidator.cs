using FluentValidation;

namespace Bookings.Modules.Ticketing.Application.Tickets.ArchiveTicketsFrorEvent;

internal sealed class ArchiveTicketsForEventCommandValidator : AbstractValidator<ArchiveTicketsForEventCommand>
{
    public ArchiveTicketsForEventCommandValidator()
    {
        RuleFor(c => c.EventId).NotEmpty();
    }
}
