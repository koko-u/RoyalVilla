using System;
using Microsoft.Extensions.DependencyInjection;

namespace RoyalVilla.Api.Extensions;

/// <summary>
/// AutoMapping Injecting Extension Methods
/// </summary>
public static class MappingProfileExtensions
{
    /// <summary>
    /// Add AutoMapping Profiles
    /// </summary>
    /// <param name="services"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public static IServiceCollection AddMappingProfiles(this IServiceCollection services, Type type)
    {
        services.AddAutoMapper(cfg => { }, type.Assembly);

        return services;
    }
}
