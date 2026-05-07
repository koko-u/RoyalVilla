namespace RoyalVilla.Api.Features.Auth.RequestData;

/// <summary>
/// Login user and password data transfer object
/// </summary>
public class LoginDto
{
    /// <summary>
    /// Email address
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// Password
    /// </summary>
    public string? Password { get; set; }
}
