using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RoyalVilla.Api.Dto;
using RoyalVilla.Api.Repositories;
using RoyalVilla.Api.Validators;

namespace RoyalVilla.Api.Controllers;

/// <summary>
/// User authentication and authorization controller
/// </summary>
[ApiController]
[Route("api/auth")]
[Tags("Auth")]
public class AuthController(UsersRepository repo) : ControllerBase
{
    /// <summary>
    /// Register a new user
    /// </summary>
    /// <param name="dto"></param>
    /// <param name="validator"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost("register")]
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
        var hasher = new PasswordHasher<RegisterUserDto>();
        var passwordHash = hasher.HashPassword(dto, dto.Password ?? throw new ArgumentNullException(nameof(dto.Password)));
        
        var createUserDto = new CreateUserDto(dto, passwordHash);
        var user = await repo.CreateUserAsync(createUserDto, cancellationToken);

        return NoContent();
    }
}