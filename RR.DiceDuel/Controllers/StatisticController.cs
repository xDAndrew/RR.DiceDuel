using Microsoft.AspNetCore.Mvc;
using RR.DiceDuel.Core.Services.StatisticService;

namespace RR.DiceDuel.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class StatisticController(IStatisticService statisticService) : ControllerBase
{
    [HttpGet(Name = "GetStatistic")]
    public IActionResult GetStatistic([FromQuery] string userName)
    {
        return Ok(statisticService.GetUserStatistic(userName));
    }
    
    [HttpGet(Name = "GetLeaders")]
    public IActionResult GetLeaders()
    {
        return Ok(statisticService.GetLeaders());
    }
}