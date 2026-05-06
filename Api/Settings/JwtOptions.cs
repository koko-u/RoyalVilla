using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace RoyalVilla.Api.Settings;

/// <summary>
/// Jwt token configuration options
/// </summary>
public class JwtOptions
{
    /// <summary>
    /// Issuer of the JWT token
    /// </summary>
    public required string Issuer { get; set; }

    /// <summary>
    /// Audience for the JWT token
    /// </summary>
    public required string Audience { get; set; }

    /// <summary>
    /// Secret key used to sign and verify JWT tokens
    /// </summary>
    [MinLength(64)]
    public required string SigningKey { get; set; }

    /// <summary>
    /// Lifetime of the access token in minutes
    /// </summary>
    public int AccessTokenMinutes { get; set; } = 15;

    /// <summary>
    /// Generate symmetric security key for JWT signing and verification
    /// </summary>
    /// <returns></returns>
    public SymmetricSecurityKey GenerateSecurityKey() => new(Encoding.UTF8.GetBytes(SigningKey));

    /// <summary>
    /// Token validation parameters for JWT verification configuration
    /// </summary>
    /// <returns></returns>
    public TokenValidationParameters GenerateValidationParameters()
    {
        return new TokenValidationParameters()
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = GenerateSecurityKey(),

            ValidateIssuer = true,
            ValidIssuer = this.Issuer,

            ValidateAudience = true,
            ValidAudience = this.Audience,

            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromMinutes(1),
        };
    }

    /// <summary>
    /// Generate JWT token with provided claims
    /// </summary>
    /// <param name="claims"></param>
    /// <returns></returns>
    public (string JwtSecurityToken, DateTimeOffset ExpiresAt) GenerateJwtToken(
        IEnumerable<Claim> claims
    )
    {
        var now = DateTimeOffset.UtcNow;
        var expiresAt = now.AddMinutes(AccessTokenMinutes);
        var credentials = new SigningCredentials(
            GenerateSecurityKey(),
            SecurityAlgorithms.HmacSha256
        );

        var token = new JwtSecurityToken(
            issuer: Issuer,
            audience: Audience,
            claims: claims,
            notBefore: now.UtcDateTime,
            expires: expiresAt.UtcDateTime,
            signingCredentials: credentials
        );

        var securityToken = new JwtSecurityTokenHandler().WriteToken(token);

        return (securityToken, expiresAt);
    }
}
