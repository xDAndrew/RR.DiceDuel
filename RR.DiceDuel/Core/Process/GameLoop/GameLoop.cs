using Microsoft.AspNetCore.SignalR;
using RR.DiceDuel.Core.Services.SessionService.Models;
using RR.DiceDuel.ExternalServices.SignalR;

namespace RR.DiceDuel.Core.Process.GameLoop;

public class GameLoop(IServiceScopeFactory scopeFactory, Session session)
{
    public void Invoke()
    {
        Task.Run(async () =>
        {
            await using var scope = scopeFactory.CreateAsyncScope();
            while (true)
            {
                var sessionHub = scope.ServiceProvider.GetRequiredService<IHubContext<GameHub>>();
                
                try
                {
                    foreach (var player in session.Players)
                    {
                        await sessionHub.Clients.Groups(session.SessionId).SendAsync("Test", $"Player in room [{session.SessionId}]: [{player.Name}]");
                    }
                
                    await Task.Delay(1000);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    await sessionHub.Clients.Groups(session.SessionId).SendAsync("Test", $"ERROR: [{e.Message}]");
                }
            }
        });
    }
}