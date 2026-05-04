using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RoyalVilla.Api.Dto;
using RoyalVilla.Api.Repositories;

namespace RoyalVilla.Api.Controllers;

[ApiController]
[Route("api/villas")]
[Tags("Villas")]
public class VillasController(VillasRepository repo, IMapper mapper) : ControllerBase
{
    [HttpGet(Name = "GetVillas")]
    public async Task<ActionResult<IEnumerable<VillaData>>> GetVillas()
    {
        var rows = await repo.GetAllVillasAsync();
        var villas = mapper.Map<IEnumerable<VillaData>>(rows);
        return Ok(villas);
    }

    [HttpGet("{id:int:min(1)}", Name = "GetVillaById")]
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