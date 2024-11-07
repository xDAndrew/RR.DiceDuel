using RR.DiceDuel.Core.Services.AuthService.Types;

namespace RR.DiceDuel.Core.Services.AuthService.Models;

public class LoginData
{
    public string Token { get; set; }
    
    public AuthStatusType Status { get; set; }
}