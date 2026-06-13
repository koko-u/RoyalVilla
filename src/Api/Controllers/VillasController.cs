using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RoyalVilla.Application.Features.Villas.Services;
using RoyalVilla.Domain.Models;

namespace RoyalVilla.Api.Controllers;

[ApiController]
[Route("api/villas")]
public sealed class VillasController(VillasService villasService) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType<IEnumerable<Villa>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Villa>>> GetVillas(CancellationToken ct)
    {
        var villas = await villasService.GetAllAsync(ct);
        return Ok(villas);
    }
}
