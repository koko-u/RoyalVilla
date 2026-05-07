using Ardalis.SmartEnum;

namespace Common.OpenApiTags;

/// <summary>
/// OpenApi document tags with descriptions
/// </summary>
/// <param name="name"></param>
/// <param name="value"></param>
public sealed class ApiTags(string name, int value) : SmartEnum<ApiTags>(name, value)
{
    /// <summary>
    /// Auth tag for authentication and authorization operations
    /// </summary>
    public static readonly ApiTags Auth = new("Auth", 1);

    /// <summary>
    /// Villas tag for villa management operations
    /// </summary>
    public static readonly ApiTags Villas = new("Villas", 2);

    /// <summary>
    /// Get Tag description
    /// </summary>
    /// <returns></returns>
    public string Description()
    {
        var description = string.Empty;
        this.When(ApiTags.Auth)
            .Then(() => description = "Authentication and Authorization Operations")
            .When(ApiTags.Villas)
            .Then(() => description = "Villa management operations")
            .Default(() => description = $"Unknown tag: {this.Name}");

        return description;
    }
}
