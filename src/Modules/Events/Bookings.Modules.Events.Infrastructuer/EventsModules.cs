using Bookings.Common.Infrastructure.Interceptors;
using Bookings.Common.Presentation.Endpoints;
using Bookings.Modules.Events.Application.Abstractions.Data;
using Bookings.Modules.Events.Domain.Categories;
using Bookings.Modules.Events.Domain.Events;
using Bookings.Modules.Events.Domain.TicketTypes;
using Bookings.Modules.Events.Infrastructure.Categories;
using Bookings.Modules.Events.Infrastructure.Database;
using Bookings.Modules.Events.Infrastructure.Events;
using Bookings.Modules.Events.Infrastructure.TicketTypes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Bookings.Modules.Events.Infrastructure;
public static class EventsModules
{

    public static IServiceCollection AddEventsModules(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddInfrastructure(configuration);

        services.AddEndPoints(Presentation.AssemblyRefrence.Assembly);

        return services;
    }

    private static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        string databaseConnectionString = configuration.GetConnectionString("EventsDatabase");

        services.AddDbContext<EventsDbContext>((sp, options) =>
        {
            options.UseNpgsql(databaseConnectionString,
                npgsqlOptions => npgsqlOptions
                .MigrationsHistoryTable(HistoryRepository.DefaultTableName, Schema.Events))
            .UseSnakeCaseNamingConvention()
            .AddInterceptors(sp.GetRequiredService<PublishDomainEventsInterceptor>());
        });

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<EventsDbContext>());

        services.AddScoped<IEventRepository, EventRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<ITicketTypeRepository, TicketTypeRepository>();

    }

}
