using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RR.DiceDuel.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TemplateController : ControllerBase
{
    [Authorize]
    [HttpGet(Name = "Test")]
    public IActionResult Get()
    {
        var name = User.Identity.Name;
        return Ok(name);
    }
}
