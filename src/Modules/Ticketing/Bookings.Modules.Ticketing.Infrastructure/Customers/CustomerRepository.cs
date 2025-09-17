using Bookings.Modules.Ticketing.Domain.Customers;
using Bookings.Modules.Ticketing.Infrastructure.DataBase;
using Microsoft.EntityFrameworkCore;

namespace Bookings.Modules.Ticketing.Infrastructure.Customers;

internal sealed class CustomerRepository(TicketingDbContext context) : ICustomerRepository
{
    public async Task<Customer?> GetAsync(Guid customerId, CancellationToken cancellationToken = default)
    {
        Customer? customer = await context.Customers.Where(c => c.Id == customerId).SingleOrDefaultAsync(cancellationToken);
        return customer;
    }

    public void Insert(Customer customer)
    {
        context.Customers.Add(customer);
    }
}
