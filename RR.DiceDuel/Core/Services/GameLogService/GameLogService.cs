using Microsoft.AspNetCore.SignalR;
using RR.DiceDuel.ExternalServices.SignalR;

namespace RR.DiceDuel.Core.Services.GameLogService;

public class GameLogService(IHubContext<GameHub> gameContext, ILogger<GameLogService> logger) : IGameLogService
{
    public void LogInfo(string roomId, string message)
    {
        logger.LogInformation($"{roomId}: {message}");
        gameContext.Clients.Groups(roomId).SendAsync("LogInfo", message);
    }
}