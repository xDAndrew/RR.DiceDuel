using Microsoft.AspNetCore.SignalR;
using RR.DiceDuel.Core.Services.SessionService.Models;
using RR.DiceDuel.Core.StateMachine.Interfaces;
using RR.DiceDuel.ExternalServices.SignalR;

namespace RR.DiceDuel.Core.Process;

public class GameLoop(IServiceScopeFactory scopeFactory, Session session)
{
    public void Invoke()
    {
        Task.Run(async () =>
        {
            await using var scope = scopeFactory.CreateAsyncScope();
            var sessionHub = scope.ServiceProvider.GetRequiredService<IHubContext<GameHub>>();
            var stateMachine = scope.ServiceProvider.GetRequiredService<IStateMachine>();

            var gameOngoing = true;
            while (gameOngoing)
            {
                try
                {
                    gameOngoing = stateMachine.Update(session);
                    await Task.Delay(100);
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