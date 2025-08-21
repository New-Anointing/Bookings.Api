using FluentValidation;

namespace Bookings.Modules.Events.Application.TicketTypes.UpdateTicketTypePrice;

internal sealed class UpdateTicketTypePriceCommandValidator : AbstractValidator<UpdateTicketTypePriceCommand>
{
    public UpdateTicketTypePriceCommandValidator()
    {
        RuleFor(t => t.TicketTypeId).NotEmpty();
        RuleFor(t => t.Price).NotEmpty();
    }
}
