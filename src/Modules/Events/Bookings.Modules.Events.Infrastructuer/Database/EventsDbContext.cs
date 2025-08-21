using Bookings.Modules.Events.Application.Abstractions.Data;
using Bookings.Modules.Events.Domain.Categories;
using Bookings.Modules.Events.Domain.Events;
using Bookings.Modules.Events.Domain.TicketTypes;
using Microsoft.EntityFrameworkCore;

namespace Bookings.Modules.Events.Infrastructure.Database;
public sealed class EventsDbContext(DbContextOptions<EventsDbContext> options)
    : DbContext(options), IUnitOfWork
{
    // Define DbSets for your entities here
    internal DbSet<Event> Events { get; set; }
    internal DbSet<Category> Categories { get; set; }
    internal DbSet<TicketType> TicketTypes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schema.Events);
    }
}

