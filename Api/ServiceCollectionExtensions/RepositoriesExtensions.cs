using System;
using System.Linq;
using Common.MakerInterfaces;
using Microsoft.Extensions.DependencyInjection;

namespace RoyalVilla.Api.ServiceCollectionExtensions;

/// <summary>
/// Add Repository services to the service collection based on the provided type.
/// </summary>
public static class RepositoriesExtensions
{
    /// <summary>
    /// Add All Repository services
    /// </summary>
    /// <typeparam name="T">The type to scan for repository implementations.</typeparam>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddRepositories<T>(this IServiceCollection services)
    {
        services.AddRepositories(typeof(T));

        return services;
    }

    /// <summary>
    /// Add All Repository services
    /// </summary>
    /// <param name="services"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public static IServiceCollection AddRepositories(this IServiceCollection services, Type type)
    {
        var repoTypes = type
            .Assembly.GetTypes()
            .Where(t =>
                t is { IsClass: true, IsSealed: true } && t.IsAssignableTo(typeof(IRepository))
            );
        foreach (var repo in repoTypes)
        {
            services.AddScoped(repo);
        }

        return services;
    }
}
