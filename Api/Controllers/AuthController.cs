using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using RoyalVilla.Api.Dto;
using RoyalVilla.Api.Repositories;
using RoyalVilla.Api.Services.Auth;
using RoyalVilla.Api.Settings;
using RoyalVilla.Api.Validators;

namespace RoyalVilla.Api.Controllers;

/// <summary>
/// User authentication and authorization controller
/// </summary>
[ApiController]
[Route("api/auth")]
[Tags("Auth")]
public class AuthController(
    UsersRepository repo, 
    CustomPasswordHasher passwordHasher, 
    IOptions<JwtOptions> options) : ControllerBase
{
    /// <summary>
    /// Register a new user
    /// </summary>
    /// <param name="dto"></param>
    /// <param name="validator"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost("register")]
    [
        ProducesResponseType(StatusCodes.Status204NoContent),
        ProducesResponseType(StatusCodes.Status400BadRequest),
    ]
    public async Task<ActionResult> Register(
        [FromBody] RegisterUserDto dto,
        [FromServices] IValidator<RegisterUserDto> validator, 
        CancellationToken cancellationToken)
    {
        var result = await validator.ValidateAsync(dto, cancellationToken);
        if (!result.IsValid)
        {
            ModelState.AddFluentErrorsToModelState(result.Errors);
            return ValidationProblem(ModelState);
        }

        // create hashed password
        var passwordHash = passwordHasher.HashPassword(dto.Password ?? throw new ArgumentNullException(nameof(dto.Password)));
        
        var createUserDto = new CreateUserDto(dto, passwordHash);
        var user = await repo.CreateUserAsync(createUserDto, cancellationToken);

        return NoContent();
    }

    /// <summary>
    /// Login user with provided credentials
    /// </summary>
    /// <param name="dto"></param>
    /// <param name="validator"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost("login")]
    [
        ProducesResponseType<LoginSuccessDto>(StatusCodes.Status200OK),
        ProducesResponseType(StatusCodes.Status400BadRequest),
    ]
    public async Task<ActionResult<LoginSuccessDto>> Login(
        [FromBody] LoginDto dto,
        [FromServices] IValidator<LoginDto> validator,
        CancellationToken cancellationToken)
    {
        var result = await validator.ValidateAsync(dto, cancellationToken);
        if (!result.IsValid)
        {
            ModelState.AddFluentErrorsToModelState(result.Errors);
            return Unauthorized(ModelState);
        }

        var email = dto.Email ?? throw new ArgumentNullException(nameof(dto.Email));
        var user = await repo.GetUserByEmailAsync(email, cancellationToken);
        if (user is null)
        {
            return Problem();
        }
        
        var claims = new List<Claim>
        {
            new (JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new (JwtRegisteredClaimNames.Email, user.Email),
            new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };
        claims.AddRange(user.Roles.Select(r => new Claim(ClaimTypes.Role, r.Name)));

        var (token, expiresAt) = options.Value.GenerateJwtToken(claims);
        
        Response.Headers.CacheControl = "no-cache, no-store, must-revalidate";
        Response.Headers.Pragma = "no-cache";
        
        return Ok(new LoginSuccessDto
        {
            AccessToken = token,
            ExpiresAt = expiresAt,
        });
    }
}