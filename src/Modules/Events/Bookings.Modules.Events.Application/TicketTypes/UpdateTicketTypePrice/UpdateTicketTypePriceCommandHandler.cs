using Bookings.Modules.Events.Application.Abstractions.Data;
using Bookings.Modules.Events.Application.Abstractions.Messaging;
using Bookings.Modules.Events.Domain.Abstractions;
using Bookings.Modules.Events.Domain.TicketTypes;

namespace Bookings.Modules.Events.Application.TicketTypes.UpdateTicketTypePrice;

internal class UpdateTicketTypePriceCommandHandler(
    IUnitOfWork unitOfWork,
    ITicketTypeRepository ticketTypeRepository)
    : ICommandHandler<UpdateTicketTypePriceCommand>
{
    public async Task<Result> Handle(UpdateTicketTypePriceCommand command, CancellationToken cancellationToken)
    {
        TicketType? ticketType = await ticketTypeRepository.GetAsync(command.TicketTypeId, cancellationToken);

        if (ticketType is null)
        {
            return Result.Failure(TicketTypeErrors.NotFound(command.TicketTypeId));
        }

        ticketType.UpdatePrice(command.Price);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
