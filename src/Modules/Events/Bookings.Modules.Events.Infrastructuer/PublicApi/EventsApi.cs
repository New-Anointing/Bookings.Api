using Bookings.Common.Domain;
using Bookings.Modules.Events.Application.TicketTypes.GetTicketType;
using Bookings.Modules.Events.PublicApi;
using MediatR;
using TicketTypeResponse = Bookings.Modules.Events.PublicApi.TicketTypeResponse;

namespace Bookings.Modules.Events.Infrastructure.PublicApi;

internal sealed class EventsApi(ISender sender) : IEventsApi
{
    public async Task<TicketTypeResponse?> GetTicketTypeAsync(Guid ticketTypeId, CancellationToken cancellationToken = default)
    {
        Result<Application.TicketTypes.GetTicketType.TicketTypeResponse> result =
            await sender.Send(new GetTicketTypeQuery(ticketTypeId), cancellationToken);

        if (result == null)
        {
            return null;
        }

        return new TicketTypeResponse(
            result.Value.Id,
            result.Value.EventId,
            result.Value.Name,
            result.Value.Price,
            result.Value.Currency,
            result.Value.Quantity);
    }
}
