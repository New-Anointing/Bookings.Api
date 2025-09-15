using Bookings.Common.Infrastructure.Interceptors;
using Bookings.Common.Presentation.Endpoints;
using Bookings.Modules.Users.Application.Abstractions.Data;
using Bookings.Modules.Users.Domain.Users;
using Bookings.Modules.Users.Infrastucture.Database;
using Bookings.Modules.Users.Infrastucture.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Bookings.Modules.Users.Infrastucture;

public static class UsersModule
{
    public static IServiceCollection AddUsersModule(this IServiceCollection services, IConfiguration confiruration)
    {
        services.AddInfrastructure(confiruration);

        services.AddEndPoints(Presentation.AssemblyRefrence.Assembly);

        return services;
    }

    private static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        string databaseConnectionString = configuration.GetConnectionString("Database");

        services.AddDbContext<UsersDbContext>((sp, options) =>
        {
            options.UseNpgsql(databaseConnectionString, npgsqlOptions => npgsqlOptions
                .MigrationsHistoryTable(HistoryRepository.DefaultTableName, Schema.Users));
            options.UseSnakeCaseNamingConvention();
            options.AddInterceptors(sp.GetRequiredService<PublishDomainEventsInterceptor>());
        });

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<UsersDbContext>());

        services.AddScoped<IUserRepository, UserRepository>();
    }
}
