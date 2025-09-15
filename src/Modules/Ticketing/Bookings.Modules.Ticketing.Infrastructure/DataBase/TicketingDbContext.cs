using Bookings.Modules.Ticketing.Application.Abstractions.Data;
using Bookings.Modules.Ticketing.Domain.Customers;
using Bookings.Modules.Ticketing.Infrastructure.Customers;
using Microsoft.EntityFrameworkCore;

namespace Bookings.Modules.Ticketing.Infrastructure.DataBase;

public sealed class TicketingDbContext(DbContextOptions<TicketingDbContext> options)
    : DbContext(options), IUnitOfWork
{
    internal DbSet<Customer> Customers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schema.Ticketing);
        modelBuilder.ApplyConfiguration(new CustomerConfiguration());
    }
}
