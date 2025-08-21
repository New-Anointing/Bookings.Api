using Bookings.Modules.Events.Application.Abstractions.Clock;
using Bookings.Modules.Events.Application.Abstractions.Data;
using Bookings.Modules.Events.Application.Abstractions.Messaging;
using Bookings.Modules.Events.Domain.Abstractions;
using Bookings.Modules.Events.Domain.Categories;
using Bookings.Modules.Events.Domain.Events;


namespace Bookings.Modules.Events.Application.Events.CreateEvent;

internal sealed partial class CreateEventCommandHandler
    (IEventRepository eventRepository,
    ICategoryRepository categoryRepository,
    IDateTimeProvider dateTimeProvider,
    IUnitOfWork unitOfWork) : 
    ICommandHandler<CreateEventCommand, Guid>
{

    public async Task<Result<Guid>> Handle(CreateEventCommand command, CancellationToken cancellationToken)
    {
        if (command.StartsAtUtc < dateTimeProvider.UtcNow)
        {
            return Result.Failure<Guid>(EventErrors.StartDateInPast);
        }

        Category? category = await categoryRepository.GetAsync(command.CategoryId, cancellationToken);

        if (category is null)
        {
            return Result.Failure<Guid>(CategoryErrors.NotFound(command.CategoryId));
        }
        Result<Event> result = Event.Create(
            category,
            command.Title,
            command.Description,
            command.Location,
            command.StartsAtUtc,
            command.EndsAtUtc);

        if (result.IsFailure)
        {
            return Result.Failure<Guid>(result.Error);
        }

        eventRepository.Insert(result.Value);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return result.Value.Id;
    }

}
