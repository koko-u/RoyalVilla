namespace RoyalVilla.Api.Repositories;

/// <summary>
/// User Row
/// </summary>
public class UserRow
{
    /// <summary>
    /// id
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
}

/// <summary>
/// User row with roles representation
/// </summary>
public class UserAndRoleRow
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
    /// User's role id
    /// </summary>
    public int? RoleId { get; set; }

    /// <summary>
    /// User's role name
    /// </summary>
    public string? RoleName { get; set; }

    /// <summary>
    /// Extracts user row without role information
    /// </summary>
    /// <returns></returns>
    public UserRow ExtractUserRow()
    {
        return new UserRow
        {
            Id = Id,
            Email = Email,
            DisplayName = DisplayName,
            IsActive = IsActive,
        };
    }
}
