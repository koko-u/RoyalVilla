using Microsoft.Extensions.DependencyInjection;
using RoyalVilla.Application.Features.Villas.Services;

namespace RoyalVilla.Application;

public static class ApplicationServiceCollectionExtension
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<VillasService>();

        return services;
    }
}
