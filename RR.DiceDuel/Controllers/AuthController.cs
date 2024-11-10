using Microsoft.AspNetCore.Mvc;
using RR.DiceDuel.Controllers.Models;
using RR.DiceDuel.Core.Services.AuthService;
using RR.DiceDuel.Core.Services.AuthService.Types;

namespace RR.DiceDuel.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Register([FromBody]LoginRequest loginDataRequest)
    {
        var loginData = await authService.RegisterUserAsync(loginDataRequest.Name, loginDataRequest.Password);
        if (loginData.Status == AuthStatusType.Success)
        {
            return new OkResult();
        }
        
        return new UnauthorizedResult();
    }
    
    [HttpPost]
    public async Task<IActionResult> Login([FromBody]LoginRequest loginDataRequest)
    {
        var loginData = await authService.LoginAsync(loginDataRequest.Name, loginDataRequest.Password);
        if (loginData.Status == AuthStatusType.Success)
        {
            return new OkObjectResult(loginData.Token);
        }
        
        return new UnauthorizedResult();
    }
}