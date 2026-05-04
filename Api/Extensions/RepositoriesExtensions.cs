using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using RoyalVilla.Api.Repositories;

namespace RoyalVilla.Api.Extensions;

/// <summary>
/// Add Repository services to the service collection based on the provided type.
/// </summary>
public static class RepositoriesExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection services, Type type)
    {
        var repoTypes =
            type.Assembly.GetTypes().Where(t => t is { IsClass: true, IsSealed: true } && t.IsAssignableTo(typeof(IRepository)));
        foreach (var repo in repoTypes)
        {
            services.AddScoped(repo);
        }

        return services;
    }
}