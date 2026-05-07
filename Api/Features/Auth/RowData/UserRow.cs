namespace RoyalVilla.Api.Features.Auth.RowData;

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
