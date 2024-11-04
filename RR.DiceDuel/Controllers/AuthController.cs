using Microsoft.AspNetCore.Mvc;
using RR.DiceDuel.Core.Services.AuthService;

namespace RR.DiceDuel.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost]
    [Route("[action]")]
    public async Task<IActionResult> Register([FromQuery]string userName, [FromQuery]string password)
    {
        var status = await _authService.RegisterUserAsync(userName, password);
        if (status == AuthStatusType.SUCCESS)
        {
            return new OkResult();
        }
        
        return new UnauthorizedResult();
    }
    
    [HttpGet]
    [Route("[action]")]
    public async Task<IActionResult> Login([FromQuery]string userName, [FromQuery]string password)
    {
        var authResult = await _authService.LoginAsync(userName, password);
        if (authResult.Status == AuthStatusType.SUCCESS)
        {
            return new OkObjectResult(authResult.Token);
        }
        
        return new UnauthorizedResult();
    }
}