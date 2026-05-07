using System;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace RoyalVilla.Api.Features.Auth.ResponseData;

/// <summary>
/// Successfully logged in user response object
/// </summary>
public class LoginSuccessData
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
