using Microsoft.AspNetCore.Mvc;
using RR.DiceDuel.Core.Services.AuthService;
using RR.DiceDuel.Core.Services.AuthService.Types;

namespace RR.DiceDuel.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class AuthController(IAuthService authService)
{
    [HttpPost]
    public async Task<IActionResult> Register([FromQuery]string userName, [FromQuery]string password)
    {
        var loginData = await authService.RegisterUserAsync(userName, password);
        if (loginData.Status == AuthStatusType.Success)
        {
            return new OkResult();
        }
        
        return new UnauthorizedResult();
    }
    
    [HttpPost]
    public async Task<IActionResult> Login([FromQuery]string userName, [FromQuery]string password)
    {
        var loginData = await authService.LoginAsync(userName, password);
        if (loginData.Status == AuthStatusType.Success)
        {
            return new OkObjectResult(loginData.Token);
        }
        
        return new UnauthorizedResult();
    }
}