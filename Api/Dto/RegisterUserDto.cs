using System;

namespace RoyalVilla.Api.Dto;

/// <summary>
/// New User register data
/// </summary>
public class RegisterUserDto
{
    /// <summary>
    /// email address
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// password
    /// </summary>
    public string? Password { get; set; }

    /// <summary>
    /// Password confirmation
    /// </summary>
    public string? PasswordConfirm { get; set; }

    /// <summary>
    /// Display name for the user
    /// </summary>
    public string? DisplayName { get; set; }

    /// <summary>
    /// User's role
    /// </summary>
    public string[] Roles { get; set; } = [];
}

/// <summary>
/// validated Register User data
/// </summary>
public class CreateUserDto
{
    /// <summary>
    /// email address
    /// </summary>
    public string Email { get; }

    /// <summary>
    /// password
    /// </summary>
    public string PasswordHash { get; }

    /// <summary>
    /// Display name for the user
    /// </summary>
    public string? DisplayName { get; }

    /// <summary>
    /// User's role
    /// </summary>
    public string[] Roles { get; }

    /// <summary>
    /// create from valid register user dto
    /// </summary>
    /// <param name="dto"></param>
    /// <param name="passwordHash"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public CreateUserDto(RegisterUserDto dto, string passwordHash)
    {
        Email = dto.Email ?? throw new ArgumentNullException(nameof(dto.Email));
        PasswordHash = passwordHash;
        DisplayName = dto.DisplayName;
        Roles = dto.Roles;
    }
}
