using Common.OpenApiTags;
using Microsoft.Extensions.DependencyInjection;

namespace RoyalVilla.Api.ServiceCollectionExtensions;

/// <summary>
/// Configure policy-based authorization settings extension
/// </summary>
public static class PolicyBasedAuthorizationExtension
{
    /// <summary>
    /// Add Authorization Policies
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddPolicyBasedAuthorization(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.AddPolicy(
                AuthPolicy.AdminOnly.ToString(),
                policy => policy.RequireRole("Admin")
            );
            options.AddPolicy(
                AuthPolicy.User.ToString(),
                policy => policy.RequireAuthenticatedUser()
            );
        });

        return services;
    }
}
