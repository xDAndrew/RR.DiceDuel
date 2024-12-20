﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RR.DiceDuel.Core.Services.AuthService.Models;
using RR.DiceDuel.Core.Services.AuthService.Types;
using RR.DiceDuel.Core.Services.PlayerService;
using RR.DiceDuel.ExternalServices.EntityFramework;
using RR.DiceDuel.ExternalServices.EntityFramework.Entities;

namespace RR.DiceDuel.Core.Services.AuthService;

public class AuthService(GameContext gameContext, IPlayerService playerService) : IAuthService
{
    public async Task<LoginData> RegisterUserAsync(string userName, string password)
    {
        var isUserExist = await gameContext.Users.AnyAsync(x => x.UserName == userName);
        if (isUserExist)
        {
            return new LoginData
            {
                Status = AuthStatusType.UserAlreadyExist
            };
        }

        var newUser = new UserEntity
        {
            UserName = userName,
            Password = BCrypt.Net.BCrypt.HashPassword(password)
        };

        await gameContext.Users.AddAsync(newUser);
        await gameContext.SaveChangesAsync();

        var token = GenerateJwtToken(userName);
        
        return new LoginData
        {
            Token = token,
            Status = AuthStatusType.Success
        };
    }

    public async Task<LoginData> LoginAsync(string userName, string password)
    {
        var user = await gameContext.Users.FirstOrDefaultAsync(x => x.UserName == userName);
        if (user == null)
        {
            return new LoginData
            {
                Status = AuthStatusType.UserNotFound
            };
        }
        
        var isPasswordValid = BCrypt.Net.BCrypt.Verify(password, user.Password);
        if (!isPasswordValid)
        {
            return new LoginData
            {
                Status = AuthStatusType.WrongPassword
            };
        }

        if (playerService.GetPlayers().Any(x => x.Name == userName))
        {
            return new LoginData
            {
                Status = AuthStatusType.UserAlreadyConnected
            };
        }
        
        var token = GenerateJwtToken(userName);
        
        return new LoginData
        {
            Token = token,
            Status = AuthStatusType.Success
        };
    }

    public bool VerifyJwt(string jwt)
    {
        var handler = new JwtSecurityTokenHandler();
        if (!handler.CanReadToken(jwt))
        {
            return false;
        }
        
        var jwtToken = handler.ReadJwtToken(jwt);
        var expiryDate = jwtToken.ValidTo;
        
        return expiryDate > DateTime.UtcNow;
    }

    public string GetUserName(string jwt)
    {
        var handler = new JwtSecurityTokenHandler();
        if (!handler.CanReadToken(jwt))
        {
            return string.Empty;
        }
        
        var jwtToken = handler.ReadJwtToken(jwt);
        var userName = jwtToken.Claims.FirstOrDefault(x => x.Type == "unique_name")?.Value;
        return userName;
    }

    private static string GenerateJwtToken(string username)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var secretKey = Environment.GetEnvironmentVariable("JWT_SECRET_KEY");
        var key = System.Text.Encoding.UTF8.GetBytes(secretKey);
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