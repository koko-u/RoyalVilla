using System;
using Microsoft.Extensions.DependencyInjection;

namespace RoyalVilla.Api.Extensions;

public static class MappingProfileExtensions
{
    public static IServiceCollection AddMappingProfiles(this IServiceCollection services, Type type)
    {
        services.AddAutoMapper(cfg => { }, type.Assembly);
       
        return services;
    }
}