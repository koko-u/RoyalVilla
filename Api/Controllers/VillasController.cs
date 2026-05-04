using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace RoyalVilla.Api.Controllers;

[ApiController]
[Route("api/villas")]
[Tags("Villas")]
public class VillasController : ControllerBase
{
    [HttpGet(Name = "GetVillas")]
    public ActionResult<string> GetVillas()
    {
        return Ok("Get all Villas");
    }

    [HttpGet("{id:int:min(1)}", Name = "GetVillaById")]
    public ActionResult<string> GetVillaById(int id)
    {
        return Ok($"Get villa by id: {id}");
    }
}