namespace RoyalVilla.Api.Features.Auth.ResponseData;

/// <summary>
/// user information with it's roles
/// </summary>
public sealed class UserData
{
    /// <summary>
    /// user Id
    /// </summary>
    public required int Id { get; set; }

    /// <summary>
    /// email address
    /// </summary>
    public required string Email { get; set; }

    /// <summary>
    /// Display name for the user
    /// </summary>
    public string? DisplayName { get; set; }

    /// <summary>
    /// User's activity status
    /// </summary>
    public required bool IsActive { get; set; }

    /// <summary>
    /// User's role
    /// </summary>
    public RoleData[] Roles { get; set; } = [];
}
