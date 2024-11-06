using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RR.DiceDuel.Core.Services.PlayerService;

namespace RR.DiceDuel.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TemplateController(IPlayerService playerService) : ControllerBase
{
    [Authorize]
    [HttpGet(Name = "Test")]
    public IActionResult Get()
    {
        var players = playerService.GetPlayers();
        return Ok(players);
    }
}
