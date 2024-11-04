using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RR.DiceDuel.Core.Services.AuthService.Models;
using RR.DiceDuel.ExternalServices.EntityFramework;
using RR.DiceDuel.ExternalServices.EntityFramework.Entities;

namespace RR.DiceDuel.Core.Services.AuthService;

public class AuthService : IAuthService
{
    private readonly GameContext _gameContext;

    public AuthService(GameContext gameContext)
    {
        _gameContext = gameContext;
    }

    public async Task<AuthStatusType> RegisterUserAsync(string userName, string password)
    {
        var isUserExist = await _gameContext.Users.AnyAsync(x => x.UserName == userName);
        if (isUserExist)
        {
            return AuthStatusType.USER_ALREADY_EXIST;
        }

        var newUser = new UserEntity
        {
            UserName = userName,
            Password = BCrypt.Net.BCrypt.HashPassword(password)
        };

        await _gameContext.Users.AddAsync(newUser);
        await _gameContext.SaveChangesAsync();

        return AuthStatusType.SUCCESS;
    }

    public async Task<LoginData> LoginAsync(string userName, string password)
    {
        var user = await _gameContext.Users.FirstOrDefaultAsync(x => x.UserName == userName);
        if (user == null)
        {
            return new LoginData
            {
                Status = AuthStatusType.USER_NOT_FOUND
            };
        }
        
        var isPasswordValid = BCrypt.Net.BCrypt.Verify(password, user.Password);
        if (!isPasswordValid)
        {
            return new LoginData
            {
                Status = AuthStatusType.WRONG_PASSWORD
            };
        }

        var token = GenerateJwtToken(userName);
        
        return new LoginData
        {
            Token = token,
            Status = AuthStatusType.SUCCESS
        };
    }
    
    private static string GenerateJwtToken(string username)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = "MySuperSecretKeyWithAtLeast32Characters!"u8.ToArray();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, username)
            }),
            Expires = DateTime.UtcNow.AddHours(24),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}