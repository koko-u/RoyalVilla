namespace RoyalVilla.Api.Features.Auth.ResponseData;

/// <summary>
/// Role information
/// </summary>
public sealed class RoleData
{
    /// <summary>
    /// role's id
    /// </summary>
    public required int Id { get; set; }

    /// <summary>
    /// role's name
    /// </summary>
    public required string Name { get; set; }
}
