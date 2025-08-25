using FluentValidation;

namespace Bookings.Modules.Events.Application.Events.PublishEvent;

internal sealed class PublishEventCommandValidator : AbstractValidator<PublishEventCommand>
{
    public PublishEventCommandValidator()
    {
        RuleFor(x => x.EventId).NotEmpty();
    }
}
