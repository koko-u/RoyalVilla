using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace RoyalVilla.Api.Controllers;

[ApiController]
[Route("api/villas")]
public class VillasController : ControllerBase
{
    [HttpGet]
    public ActionResult<string> GetVillas()
    {
        return Ok("Get all Villas");
    }
}