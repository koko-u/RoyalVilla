using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RoyalVilla.Api.Dto;
using RoyalVilla.Api.Repositories;

namespace RoyalVilla.Api.Controllers;

/// <summary>
/// Villas controller for managing villa data
/// </summary>
/// <param name="repo"></param>
/// <param name="mapper"></param>
[ApiController]
[Route("api/villas")]
[Tags("Villas")]
public class VillasController(VillasRepository repo, IMapper mapper) : ControllerBase
{
    /// <summary>
    /// Get All Villas Data
    /// </summary>
    /// <returns></returns>
    [HttpGet(Name = "GetVillas")]
    [ProducesResponseType<IEnumerable<VillaData>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<VillaData>>> GetVillas()
    {
        var rows = await repo.GetAllVillasAsync();
        var villas = mapper.Map<IEnumerable<VillaData>>(rows);
        return Ok(villas);
    }

    /// <summary>
    /// Get Villa by ID
    /// </summary>
    /// <param name="id">Villa ID</param>
    /// <returns></returns>
    [HttpGet("{id:int:min(1)}", Name = "GetVillaById")]
    [ProducesResponseType<VillaData>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<VillaData>> GetVillaById(int id)
    {
        var row = await repo.GetVillaByIdAsync(id);
        if (row is null)
        {
            return NotFound($"Villa with id {id} not found");
        }

        var villa = mapper.Map<VillaData>(row);
        return Ok(villa);
    }
}