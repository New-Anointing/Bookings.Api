
using Bookings.Common.Presentation.Endpoints;
using Bookings.Modules.Ticketing.Application.Carts;
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

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "<Pending>")]
    private static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        //Add infrastructure services
        services.AddSingleton<CartService>();
    }
}
