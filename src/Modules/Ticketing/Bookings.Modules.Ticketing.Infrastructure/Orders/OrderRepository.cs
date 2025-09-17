using Bookings.Modules.Ticketing.Domain.Orders;
using Bookings.Modules.Ticketing.Infrastructure.DataBase;
using Microsoft.EntityFrameworkCore;

namespace Bookings.Modules.Ticketing.Infrastructure.Orders;

internal sealed class OrderRepository(TicketingDbContext context) : IOrderRepository
{
    public async Task<Order?> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Orders.Include(o => o.OrderItems).SingleOrDefaultAsync(o => o.Id == id, cancellationToken);
    }

    public void Insert(Order order)
    {
        context.Orders.Add(order);
    }
}
