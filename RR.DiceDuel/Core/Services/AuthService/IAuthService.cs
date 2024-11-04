using RR.DiceDuel.Core.Services.AuthService.Models;

namespace RR.DiceDuel.Core.Services.AuthService;

public interface IAuthService
{
    Task<AuthStatusType> RegisterUserAsync(string userName, string password);
    
    Task<LoginData> LoginAsync(string userName, string password);
}