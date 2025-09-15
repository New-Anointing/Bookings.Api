using Bookings.Modules.Users.Application.Abstractions.Data;
using Bookings.Modules.Users.Domain.Users;
using Bookings.Modules.Users.Infrastucture.Users;
using Microsoft.EntityFrameworkCore;

namespace Bookings.Modules.Users.Infrastucture.Database;

public sealed class UsersDbContext(DbContextOptions<UsersDbContext> options)
    : DbContext(options), IUnitOfWork
{
    internal DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schema.Users);
        modelBuilder.ApplyConfiguration(new UserConfiguration());
    }
}
