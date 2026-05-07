using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RoyalVilla.Api.Extensions;
using RoyalVilla.Api.Features.Auth.RequestData;
using RoyalVilla.Api.Features.Auth.ResponseData;

namespace RoyalVilla.Api.Features.Auth;

/// <summary>
/// User authentication and authorization controller
/// </summary>
[ApiController]
[Route("api/auth")]
[Tags("Auth")]
public class AuthController(AuthService authService) : ControllerBase
{
    /// <summary>
    /// Register a new user
    /// </summary>
    /// <param name="dto"></param>
    /// <param name="validator"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost("register")]
    [AllowAnonymous]
    [
        ProducesResponseType(StatusCodes.Status204NoContent),
        ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest),
    ]
    public async Task<ActionResult> Register(
        [FromBody] RegisterUserDto dto,
        [FromServices] IValidator<RegisterUserDto> validator,
        CancellationToken cancellationToken
    )
    {
        var result = await validator.ValidateAsync(dto, cancellationToken);
        if (!result.IsValid)
        {
            ModelState.AddFluentErrorsToModelState(result.Errors);
            return ValidationProblem(ModelState);
        }

        var registeredUser = await authService.RegisterUserAsync(dto, cancellationToken);
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
    [AllowAnonymous]
    [
        ProducesResponseType<LoginSuccessData>(StatusCodes.Status200OK),
        ProducesResponseType<UnauthorizedObjectResult>(StatusCodes.Status401Unauthorized),
        ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError),
    ]
    public async Task<ActionResult<LoginSuccessData>> Login(
        [FromBody] LoginDto dto,
        [FromServices] IValidator<LoginDto> validator,
        CancellationToken cancellationToken
    )
    {
        var result = await validator.ValidateAsync(dto, cancellationToken);
        if (!result.IsValid)
        {
            ModelState.AddFluentErrorsToModelState(result.Errors);
            return Unauthorized(ModelState);
        }

        var successData = await authService.VerifyUserAsync(dto, cancellationToken);
        if (successData is null)
        {
            return Problem(
                title: "Failed to verify user by email and password",
                detail: "The provided email and password does not match.",
                statusCode: StatusCodes.Status500InternalServerError
            );
        }

        Response.Headers.CacheControl = "no-cache, no-store, must-revalidate";
        Response.Headers.Pragma = "no-cache";

        return Ok(successData);
    }
}
