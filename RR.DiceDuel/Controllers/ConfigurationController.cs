using Microsoft.AspNetCore.Mvc;
using RR.DiceDuel.Core.Services.ConfigurationSerivce;
using RR.DiceDuel.Core.Services.ConfigurationSerivce.Models;

namespace RR.DiceDuel.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ConfigurationController(IConfigurationService configurationService) : ControllerBase
{
    [HttpPost]
    public void Set(AppConfiguration data)
    {
        configurationService.SetConfiguration(data);
    }

    [HttpGet]
    public IActionResult Get()
    {
        return new OkObjectResult(configurationService.GetConfiguration());
    }
}