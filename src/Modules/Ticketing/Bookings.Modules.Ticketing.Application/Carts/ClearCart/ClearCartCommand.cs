using Bookings.Common.Application.Messaging;

namespace Bookings.Modules.Ticketing.Application.Carts.ClearCart;

public sealed record ClearCartCommand(Guid CustomerId) : ICommand;
