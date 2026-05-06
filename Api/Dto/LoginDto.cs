using System;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace RoyalVilla.Api.Dto;

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

/// <summary>
/// Successfully logged in user response object
/// </summary>
public class LoginSuccessDto
{
    /// <summary>
    /// JWT Access token
    /// </summary>
    public required string AccessToken { get; set; }
    
    /// <summary>
    /// Expiration date and time of the access token
    /// </summary>
    public required DateTimeOffset ExpiresAt { get; set; }
    
    /// <summary>
    /// Authentication scheme for JWT bearer tokens
    /// </summary>
    public string TokenType => JwtBearerDefaults.AuthenticationScheme;
}