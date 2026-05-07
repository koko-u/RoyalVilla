using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Common.MakerInterfaces;
using Microsoft.Extensions.Options;
using RoyalVilla.Api.Features.Auth.RequestData;
using RoyalVilla.Api.Features.Auth.ResponseData;
using RoyalVilla.Api.Features.Shared;
using RoyalVilla.Api.Settings;

namespace RoyalVilla.Api.Features.Auth;

/// <summary>
/// Authentication service for handling user authentication and authorization.
/// </summary>
public sealed class AuthService(
    UsersRepository repo,
    CustomPasswordHasher passwordHasher,
    IOptions<JwtOptions> options
) : IService
{
    /// <summary>
    /// register users table with password hashing
    /// </summary>
    /// <param name="dto"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public async Task<UserData> RegisterUserAsync(
        RegisterUserDto dto,
        CancellationToken cancellationToken
    )
    {
        // create hashed password
        var passwordHash = passwordHasher.HashPassword(
            dto.Password ?? throw new ArgumentNullException(nameof(dto.Password))
        );

        var createUserDto = new CreateUserDto(dto, passwordHash);
        var user = await repo.CreateUserAsync(createUserDto, cancellationToken);

        return user;
    }

    /// <summary>
    /// authenticate user with email and password
    /// </summary>
    /// <param name="dto"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public async Task<LoginSuccessData?> VerifyUserAsync(
        LoginDto dto,
        CancellationToken cancellationToken
    )
    {
        var email = dto.Email ?? throw new ArgumentNullException(nameof(dto.Email));
        var user = await repo.GetUserByEmailAsync(email, cancellationToken);
        if (user is null)
        {
            return null;
        }

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };
        claims.AddRange(user.Roles.Select(r => new Claim(ClaimTypes.Role, r.Name)));

        var (token, expiresAt) = options.Value.GenerateJwtToken(claims);

        return new LoginSuccessData { AccessToken = token, ExpiresAt = expiresAt };
    }
}
