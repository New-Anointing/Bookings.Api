using System.Reflection;
using Bookings.Common.Application.Behaviors;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Bookings.Common.Application;

public static class ApplicationConfiguration
{
    public static IServiceCollection AddApplication(this IServiceCollection services, Assembly[] modeuleAssemblies)
    {
        services.AddMediatR(options =>
        {
            options.RegisterServicesFromAssemblies(modeuleAssemblies);
            options.AddOpenBehavior(typeof(ExceptionHandlingPipelineBehavior<,>));
            options.AddOpenBehavior(typeof(RequestLoggingPipelineBehavior<,>));
            options.AddOpenBehavior(typeof(ValidationPipelineBehavior<,>));
        });

        services.AddValidatorsFromAssemblies(modeuleAssemblies, includeInternalTypes: true);

        return services;
    }
}
