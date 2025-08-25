using FluentValidation;

namespace Bookings.Modules.Events.Application.Events.RescheduleEvent;

internal sealed class RescheduleEventCommandValidator : AbstractValidator<RescheduleEventCommand>
{
    public RescheduleEventCommandValidator()
    {
        RuleFor(x => x.EventId).NotEmpty();
        RuleFor(x => x.NewStartsAtUtc).GreaterThan(DateTime.MinValue);
        RuleFor(x => x.NewEndsAtUtc)
            .GreaterThan(DateTime.MinValue)
            .When(x => x.NewEndsAtUtc.HasValue);
        RuleFor(x => x)
            .Must(x => !x.NewEndsAtUtc.HasValue || x.NewEndsAtUtc > x.NewStartsAtUtc)
            .WithMessage("End date must be greater than start date.");
    }
}
