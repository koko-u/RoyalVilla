using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RoyalVilla.Api.Extensions;
using RoyalVilla.Api.Features.Villas.RequestData;
using RoyalVilla.Api.Features.Villas.ResponseData;

namespace RoyalVilla.Api.Features.Villas;

/// <summary>
/// Villas controller for managing villa data
/// </summary>
/// <param name="villasService"></param>
/// <param name="logger"></param>
[ApiController]
[Route("api/villas")]
[Tags("Villas")]
public class VillasController(VillasService villasService, ILogger<VillasController> logger)
    : ControllerBase
{
    /// <summary>
    /// Get All Villas Data
    /// </summary>
    /// <param name="pageQuery">Pagination query parameters</param>
    /// <param name="cancellationToken">Cancellation token for asynchronous operations</param>
    /// <returns></returns>
    [HttpGet(Name = "GetVillas")]
    [AllowAnonymous]
    [ProducesResponseType<IEnumerable<VillaData>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<VillaData>>> GetVillas(
        [FromQuery] PageQuery pageQuery,
        CancellationToken cancellationToken
    )
    {
        var villas = await villasService.GetVillasWithPagingAsync(pageQuery, cancellationToken);
        return Ok(villas);
    }

    /// <summary>
    /// Get Villa by ID
    /// </summary>
    /// <param name="id">Villa ID</param>
    /// <param name="cancellationToken">Cancellation token for asynchronous operations</param>
    /// <returns></returns>
    [HttpGet("{id:int:min(1)}", Name = "GetVillaById")]
    [
        ProducesResponseType<VillaData>(StatusCodes.Status200OK),
        ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)
    ]
    public async Task<ActionResult<VillaData>> GetVillaById(
        int id,
        CancellationToken cancellationToken
    )
    {
        var villa = await villasService.GetVillaByIdAsync(id, cancellationToken);
        if (villa is null)
        {
            return Problem(
                title: "Villa not found",
                detail: $"Villa with id {id} not found",
                statusCode: StatusCodes.Status404NotFound
            );
        }

        return Ok(villa);
    }

    /// <summary>
    /// Create a new Villa
    /// </summary>
    /// <param name="dto">create new villa data</param>
    /// <param name="validator">villa data validator</param>
    /// <param name="cancellationToken">cancellation token</param>
    [HttpPost]
    [Authorize(Roles = "User,Admin")]
    [
        ProducesResponseType<VillaData>(StatusCodes.Status201Created),
        ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)
    ]
    public async Task<ActionResult> CreateVilla(
        [FromBody] CreateOrUpdateVillaDto dto,
        [FromServices] IValidator<CreateOrUpdateVillaDto> validator,
        CancellationToken cancellationToken
    )
    {
        logger.LogInformation("Creating new villa with DTO: {dto}", dto);

        var result = await validator.ValidateAsync(dto, cancellationToken);
        if (!result.IsValid)
        {
            ModelState.AddFluentErrorsToModelState(result.Errors);
            return ValidationProblem(ModelState);
        }

        var villa = await villasService.CreateVillaAsync(dto, cancellationToken);
        return CreatedAtRoute(nameof(GetVillaById), new { id = villa.Id }, villa);
    }

    /// <summary>
    /// Update Villa Data
    /// </summary>
    /// <param name="id">target villa id</param>
    /// <param name="dto">update villa data</param>
    /// <param name="validator">validator for update data</param>
    /// <param name="cancellationToken">cancellation token</param>
    /// <returns></returns>
    [HttpPut("{id:int:min(1)}")]
    [Authorize(Roles = "User,Admin")]
    [
        ProducesResponseType<VillaData>(StatusCodes.Status200OK),
        ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound),
        ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest),
        ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError),
    ]
    public async Task<ActionResult> UpdateVilla(
        int id,
        [FromBody] CreateOrUpdateVillaDto dto,
        [FromServices] IValidator<CreateOrUpdateVillaDto> validator,
        CancellationToken cancellationToken
    )
    {
        if (await villasService.GetVillaByIdAsync(id, cancellationToken) is null)
        {
            return Problem(
                title: "Villa not found",
                detail: $"Target villa of id = {id} is not found.",
                statusCode: StatusCodes.Status404NotFound
            );
        }

        var result = await validator.ValidateAsync(dto, cancellationToken);
        if (!result.IsValid)
        {
            ModelState.AddFluentErrorsToModelState(result.Errors);
            return ValidationProblem(ModelState);
        }

        var updatedVilla = await villasService.UpdateVillaByIdAsync(id, dto, cancellationToken);
        if (updatedVilla is null)
        {
            return Problem(
                title: "Failed to update villa",
                statusCode: StatusCodes.Status500InternalServerError
            );
        }

        return Ok(updatedVilla);
    }

    /// <summary>
    /// Delete villa by id
    /// </summary>
    /// <param name="id">villa id</param>
    /// <param name="cancellationToken">cancellation token</param>
    /// <returns></returns>
    [HttpDelete("{id:int:min(1)}")]
    [Authorize(Roles = "Admin")]
    [
        ProducesResponseType<VillaData>(StatusCodes.Status200OK),
        ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound),
        ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError),
    ]
    public async Task<ActionResult> DeleteVilla(int id, CancellationToken cancellationToken)
    {
        if (await villasService.GetVillaByIdAsync(id, cancellationToken) is null)
        {
            return Problem(
                title: "Villa not found",
                detail: $"Target villa of id = {id} is not found.",
                statusCode: StatusCodes.Status404NotFound
            );
        }

        var deleteVilla = await villasService.DeleteVillaByIdAsync(id, cancellationToken);
        if (deleteVilla is null)
        {
            return Problem(
                title: "Failed to delete the villa",
                statusCode: StatusCodes.Status500InternalServerError
            );
        }

        return Ok(deleteVilla);
    }
}
