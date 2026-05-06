using System;
using System.ComponentModel.DataAnnotations;
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
    /// Generate signing credentials for JWT token
    /// </summary>
    /// <example>
    ///
    /// var token = new JwtSecurityToken(
    ///     issuer: options.Issuer,
    ///     audience: options.Audience,
    ///     claims: ...,
    ///     expires: DateTime.UtcNow.AddMinutes(options.AccessTokenMinutes),
    ///     signingCredentials: options.SigningCredentials()
    /// );
    /// 
    /// </example>
    /// <returns></returns>
    public SigningCredentials GenerateSigningCredentials() => 
        new(GenerateSecurityKey(), SecurityAlgorithms.HmacSha256); 
}