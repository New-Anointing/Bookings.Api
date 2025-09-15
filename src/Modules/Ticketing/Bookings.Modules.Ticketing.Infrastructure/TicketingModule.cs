
using Bookings.Common.Infrastructure.Interceptors;
using Bookings.Common.Presentation.Endpoints;
using Bookings.Modules.Ticketing.Application.Abstractions.Data;
using Bookings.Modules.Ticketing.Application.Carts;
using Bookings.Modules.Ticketing.Domain.Customers;
using Bookings.Modules.Ticketing.Infrastructure.Customers;
using Bookings.Modules.Ticketing.Infrastructure.DataBase;
using Bookings.Modules.Ticketing.Presentation.Customers;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Bookings.Modules.Ticketing.Infrastructure;

public static class TicketingModule
{
    public static IServiceCollection AddTicketingModule(this IServiceCollection services, IConfiguration confiruration)
    {
        services.AddInfrastructure(confiruration);

        services.AddEndPoints(Presentation.AssemblyRefrence.Assembly);

        return services;
    }

    public static void ConfigureConsumers(IRegistrationConfigurator registrationConfigurator)
    {
        registrationConfigurator.AddConsumer<UserRegisteredIntegrationEventConsumer>();
    }

    private static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        string databaseConnectionString = configuration.GetConnectionString("Database");
        //Add infrastructure services
        services.AddDbContext<TicketingDbContext>((sp, options) =>
        {
            options.UseNpgsql(databaseConnectionString, npgSqlOptions =>
            {
                npgSqlOptions.MigrationsHistoryTable(HistoryRepository.DefaultTableName, Schema.Ticketing);
            });
            options.UseSnakeCaseNamingConvention();
            options.AddInterceptors(sp.GetRequiredService<PublishDomainEventsInterceptor>());
        });

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<TicketingDbContext>());
        services.AddScoped<ICustomerRepository, CustomerRepository>();

        services.AddSingleton<CartService>();
    }
}
