using Bookings.Common.Application.Caching;
using Bookings.Common.Application.Clock;
using Bookings.Common.Application.Data;
using Bookings.Common.Infrastructure.Caching;
using Bookings.Common.Infrastructure.Clock;
using Bookings.Common.Infrastructure.Data;
using Bookings.Common.Infrastructure.Interceptors;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Npgsql;
using StackExchange.Redis;

namespace Bookings.Common.Infrastructure;

public static class InfrastructureConfiguration
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        string databaseConnectionString,
        string redisConnectionString)
    {
        NpgsqlDataSource npgsqlDataSource = new NpgsqlDataSourceBuilder(databaseConnectionString).Build();

        services.TryAddSingleton(npgsqlDataSource);

        services.AddScoped<IDbConnectionFactory, DbConnectionFactory>();

        services.TryAddSingleton<IDateTimeProvider, DateTimeProvider>();

        try
        {
            IConnectionMultiplexer connectionMultiplexer = ConnectionMultiplexer.Connect(redisConnectionString);
            services.TryAddSingleton(connectionMultiplexer);

            services.AddStackExchangeRedisCache(options =>
            {
                options.ConnectionMultiplexerFactory = () => Task.FromResult(connectionMultiplexer);
            });
        }
        catch
        {
            // If Redis is not available, we skip caching setup.
            services.AddDistributedMemoryCache();
        }
        services.TryAddSingleton<ICacheService, CacheService>();
        services.TryAddSingleton<PublishDomainEventsInterceptor>();

        return services;
    }
}
