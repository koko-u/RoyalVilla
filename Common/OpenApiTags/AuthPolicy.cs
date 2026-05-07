using Ardalis.SmartEnum;

namespace Common.OpenApiTags;

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
