using Ardalis.SmartEnum;
using Microsoft.Extensions.DependencyInjection;

namespace RoyalVilla.Api.Extensions;

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

/// <summary>
/// User Policy
/// </summary>
/// <param name="name"></param>
/// <param name="value"></param>
public sealed class AuthPolicy(string name, int value) : SmartEnum<AuthPolicy>(name, value)
{
    /// <summary>
    /// Administrator User Only
    /// </summary>
    public static readonly AuthPolicy AdminOnly = new(nameof(AdminOnly), 1);

    /// <summary>
    /// Normal User
    /// </summary>
    public static readonly AuthPolicy User = new(nameof(User), 2);

    /// <summary>
    /// Guest (non login) User
    /// </summary>
    public static readonly AuthPolicy Guest = new(nameof(Guest), 3);
}
