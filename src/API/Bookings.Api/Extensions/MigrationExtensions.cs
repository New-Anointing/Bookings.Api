using Bookings.Modules.Events.Api.Database;
using Microsoft.EntityFrameworkCore;

namespace Bookings.Api.Extensions;

internal static class MigrationExtensions
{
    internal static IApplicationBuilder ApplyMigrations(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();
        ApplyMigration<EventsDbContext>(scope);
        return app;
    }

    private static void ApplyMigration<TDbContext>(IServiceScope scope)
        where TDbContext : DbContext
    {
        TDbContext dbContext = scope.ServiceProvider.GetRequiredService<TDbContext>();
        dbContext.Database.Migrate();
    }
}
