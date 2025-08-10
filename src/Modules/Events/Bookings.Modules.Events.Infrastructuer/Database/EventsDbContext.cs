using Bookings.Modules.Events.Application.Abstractions.Data;
using Bookings.Modules.Events.Domain.Events;
using Microsoft.EntityFrameworkCore;

namespace Bookings.Modules.Events.Infrastructure.Database;
public sealed class EventsDbContext(DbContextOptions<EventsDbContext> options)
    : DbContext(options), IUnitOfWork
{
    // Define DbSets for your entities here
    internal DbSet<Event> Events { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schema.Events);
    }
}

