using System;
using System.Linq;
using Common.MakerInterfaces;
using Microsoft.Extensions.DependencyInjection;

namespace RoyalVilla.Api.ServiceCollectionExtensions;

/// <summary>
/// Add Service Layer to the service collection based on the provided type.
/// </summary>
public static class ServicesExtensions
{
    /// <summary>
    /// Add All Services
    /// </summary>
    /// <param name="services"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static IServiceCollection AddServices<T>(this IServiceCollection services)
    {
        services.AddServices(typeof(T));

        return services;
    }

    /// <summary>
    /// Add All Services
    /// </summary>
    /// <param name="services"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public static IServiceCollection AddServices(this IServiceCollection services, Type type)
    {
        var srvTypes = type
            .Assembly.GetTypes()
            .Where(t =>
                t is { IsClass: true, IsSealed: true } && t.IsAssignableTo(typeof(IService))
            );
        foreach (var srv in srvTypes)
        {
            services.AddScoped(srv);
        }

        return services;
    }
}
