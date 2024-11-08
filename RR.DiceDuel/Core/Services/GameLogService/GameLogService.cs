using Microsoft.AspNetCore.SignalR;
using RR.DiceDuel.ExternalServices.SignalR;

namespace RR.DiceDuel.Core.Services.GameLogService;

public class GameLogService(IHubContext<GameHub> gameContext) : IGameLogService
{
    public void LogInfo(string roomId, string message)
    {
        var logDirectory = Path.Combine(Path.GetTempPath(), "DiceDuel");
        var filePath = Path.Combine(logDirectory, $"{roomId}.txt");

        if (!Directory.Exists(logDirectory))
        {
            Directory.CreateDirectory(logDirectory);
        }
        
        File.AppendAllText(filePath, $"{DateTime.UtcNow}: {message}" + Environment.NewLine);
        
        gameContext.Clients.Groups(roomId).SendAsync("LogInfo", message);
    }
}