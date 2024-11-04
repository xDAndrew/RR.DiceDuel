using Microsoft.AspNetCore.Mvc;

namespace RR.DiceDuel.Controllers;

[ApiController]
[Route("[controller]")]
public class TemplateController : ControllerBase
{
    [HttpGet(Name = "GetWeatherForecast")]
    public IActionResult Get()
    {
        return Ok();
    }
}
