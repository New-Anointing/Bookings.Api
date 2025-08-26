using Bookings.Modules.Events.Application.Abstractions.Clock;
using Bookings.Modules.Events.Application.Abstractions.Data;
using Bookings.Modules.Events.Domain.Categories;
using Bookings.Modules.Events.Domain.Events;
using Bookings.Modules.Events.Domain.TicketTypes;
using Bookings.Modules.Events.Infrastructure.Categories;
using Bookings.Modules.Events.Infrastructure.Clock;
using Bookings.Modules.Events.Infrastructure.Database;
using Bookings.Modules.Events.Infrastructure.Events;
using Bookings.Modules.Events.Infrastructure.TicketTypes;
using Bookings.Modules.Events.Presentation.Events;
using FluentValidation;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Npgsql;

namespace Bookings.Modules.Events.Infrastructure;
public static class EventsModules
{
    public static void MapEndpoints(IEndpointRouteBuilder app)
    {
        EventEndpoints.MapEndpoints(app);
    }

    public static IServiceCollection AddEventsModules(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(options =>
        {
            options.RegisterServicesFromAssembly(Application.AssemblyRefrence.Assembly);
        });

        services.AddValidatorsFromAssembly(Application.AssemblyRefrence.Assembly, includeInternalTypes: true);

        services.AddInfrastructure(configuration);

        return services;
    }

    private static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        string databaseConnectionString = configuration.GetConnectionString("EventsDatabase");

        NpgsqlDataSource npgsqlDataSource = new NpgsqlDataSourceBuilder(databaseConnectionString).Build();
        services.TryAddSingleton(npgsqlDataSource);

        services.AddScoped<IDbConnectionFactory, IDbConnectionFactory>();

        services.TryAddSingleton<IDateTimeProvider, DateTimeProvider>();

        services.AddDbContext<EventsDbContext>(options =>
        {
            options.UseNpgsql(databaseConnectionString,
                npgsqlOptions => npgsqlOptions
                .MigrationsHistoryTable(HistoryRepository.DefaultTableName, Schema.Events))
            .UseSnakeCaseNamingConvention()
            .AddInterceptors();
        });

        services.AddScoped<IEventRepository, EventRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<ITicketTypeRepository, TicketTypeRepository>();

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<EventsDbContext>());
    }

}
