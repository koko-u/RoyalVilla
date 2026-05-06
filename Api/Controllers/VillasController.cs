using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RoyalVilla.Api.Dto;
using RoyalVilla.Api.Repositories;
using RoyalVilla.Api.Validators;

namespace RoyalVilla.Api.Controllers;

/// <summary>
/// Villas controller for managing villa data
/// </summary>
/// <param name="repo"></param>
/// <param name="logger"></param>
[ApiController]
[Route("api/villas")]
[Tags("Villas")]
public class VillasController(VillasRepository repo, ILogger<VillasController> logger)
    : ControllerBase
{
    /// <summary>
    /// Get All Villas Data
    /// </summary>
    /// <param name="cancellationToken">Cancellation token for asynchronous operations</param>
    /// <returns></returns>
    [HttpGet(Name = "GetVillas")]
    [AllowAnonymous]
    [ProducesResponseType<IEnumerable<VillaData>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<VillaData>>> GetVillas(
        CancellationToken cancellationToken
    )
    {
        var rows = await repo.GetAllVillasAsync(cancellationToken);

        var villas = rows.Select(r => r.ToVillaData());
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
        ProducesResponseType(StatusCodes.Status404NotFound)
    ]
    public async Task<ActionResult<VillaData>> GetVillaById(
        int id,
        CancellationToken cancellationToken
    )
    {
        var row = await repo.GetVillaByIdAsync(id, cancellationToken);
        if (row is null)
        {
            return NotFound($"Villa with id {id} not found");
        }

        return Ok(row.ToVillaData());
    }

    /// <summary>
    /// Create new Villa
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "User,Admin")]
    [
        ProducesResponseType<string>(StatusCodes.Status201Created),
        ProducesResponseType(StatusCodes.Status400BadRequest)
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

        var row = await repo.CreateVilla(dto, cancellationToken);
        return CreatedAtRoute(nameof(GetVillaById), new { id = row.Id }, "Successfully Created.");
    }

    /// <summary>
    /// Update Villa Data
    /// </summary>
    /// <param name="id"></param>
    /// <param name="dto"></param>
    /// <param name="validator"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPut("{id:int:min(1)}")]
    [Authorize(Roles = "User,Admin")]
    [
        ProducesResponseType(StatusCodes.Status204NoContent),
        ProducesResponseType(StatusCodes.Status404NotFound),
        ProducesResponseType(StatusCodes.Status400BadRequest),
        ProducesResponseType(StatusCodes.Status500InternalServerError),
    ]
    public async Task<ActionResult> UpdateVilla(
        int id,
        [FromBody] CreateOrUpdateVillaDto dto,
        [FromServices] IValidator<CreateOrUpdateVillaDto> validator,
        CancellationToken cancellationToken
    )
    {
        if (await repo.GetVillaByIdAsync(id, cancellationToken) is null)
        {
            return NotFound($"Target villa of id = {id} is not found.");
        }

        var result = await validator.ValidateAsync(dto, cancellationToken);
        if (!result.IsValid)
        {
            ModelState.AddFluentErrorsToModelState(result.Errors);
            return ValidationProblem(ModelState);
        }

        var updatedVilla = await repo.UpdateVilla(id, dto, cancellationToken);
        if (updatedVilla is null)
        {
            return Problem();
        }

        return NoContent();
    }

    /// <summary>
    /// Delete villa by id
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpDelete("{id:int:min(1)}")]
    [Authorize(Roles = "Admin")]
    [
        ProducesResponseType(StatusCodes.Status204NoContent),
        ProducesResponseType(StatusCodes.Status404NotFound),
        ProducesResponseType(StatusCodes.Status500InternalServerError),
    ]
    public async Task<ActionResult> DeleteVilla(int id, CancellationToken cancellationToken)
    {
        if (await repo.GetVillaByIdAsync(id, cancellationToken) is null)
        {
            return NotFound($"Target villa of id = {id} is not found.");
        }

        var deleteVilla = await repo.DeleteVilla(id, cancellationToken);
        if (deleteVilla is null)
        {
            return Problem();
        }

        return NoContent();
    }
}
