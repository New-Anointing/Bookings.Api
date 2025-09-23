using Bookings.Common.Application.Caching;
using Bookings.Common.Application.Clock;
using Bookings.Common.Application.Data;
using Bookings.Common.Application.EventBus;
using Bookings.Common.Infrastructure.Authentication;
using Bookings.Common.Infrastructure.Caching;
using Bookings.Common.Infrastructure.Clock;
using Bookings.Common.Infrastructure.Data;
using Bookings.Common.Infrastructure.Interceptors;
using MassTransit;
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
        Action<IRegistrationConfigurator>[] moduleConfugureConsumers,
        string redisConnectionString)
    {
        services.AddAuthenticationInternal();

        NpgsqlDataSource npgsqlDataSource = new NpgsqlDataSourceBuilder(databaseConnectionString).Build();

        services.TryAddSingleton(npgsqlDataSource);

        services.TryAddScoped<IDbConnectionFactory, DbConnectionFactory>();

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
            // If Redis is not available, we skip caching setup and add distributed memory cache.
            services.AddDistributedMemoryCache();
        }
        services.TryAddSingleton<ICacheService, CacheService>();
        services.TryAddSingleton<PublishDomainEventsInterceptor>();
        services.TryAddSingleton<IEventBus, EventBus.EventBus>();

        services.AddMassTransit((configure) =>
        {
            foreach (Action<IRegistrationConfigurator> configureCustomer in moduleConfugureConsumers)
            {
                configureCustomer(configure);
            }
            configure.SetKebabCaseEndpointNameFormatter();

            configure.UsingInMemory((context, cfg) =>
            {
                cfg.ConfigureEndpoints(context);
            });
        });


        return services;
    }
}
