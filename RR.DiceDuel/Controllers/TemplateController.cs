using Microsoft.AspNetCore.Mvc;
using RR.DiceDuel.Core.Services.PlayerService;
using RR.DiceDuel.Core.Services.SessionService;

namespace RR.DiceDuel.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class TemplateController(IPlayerService playerService, ISessionService sessionService) : ControllerBase
{
    [HttpGet(Name = "GetPlayers")]
    public IActionResult GetPlayers()
    {
        var players = playerService.GetPlayers();
        return Ok(players);
    }
    
    [HttpGet(Name = "GetSessions")]
    public IActionResult GetSessions()
    {
        var sessions = sessionService.GetSessions();
        return Ok(sessions);
    }
}
