using WildPathApp.Infrastructure.Repositories;
using WildPathApp.Application.Interfaces;
using WildPathApp.Application.Services;
using WildPathApp.Core.Interfaces;

namespace WildPathApp.API.Extensions;

public static class ServiceRegistrationExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IEventRepository, EventRepository>();

        //...other repositories may be added here
        return services;
    }

    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IEventService, EventService>();

        //...other services may be added here
        return services;
    }
}
