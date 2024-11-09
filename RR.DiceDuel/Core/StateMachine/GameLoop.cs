using RR.DiceDuel.Core.Services.GameLogService;
using RR.DiceDuel.Core.Services.SessionService;
using RR.DiceDuel.Core.StateMachine.Interfaces;

namespace RR.DiceDuel.Core.StateMachine;

public class GameLoop(IServiceScopeFactory scopeFactory, string sessionId)
{
    public void Invoke()
    {
        Task.Run(async () =>
        {
            await using var scope = scopeFactory.CreateAsyncScope();
            
            var logger = scope.ServiceProvider.GetRequiredService<IGameLogService>();
            var stateMachine = scope.ServiceProvider.GetRequiredService<IStateMachine>();
            var sessionService = scope.ServiceProvider.GetRequiredService<ISessionService>();

            var gameOngoing = true;
            while (gameOngoing)
            {
                try
                {
                    gameOngoing = stateMachine.Update(sessionId, scope);
                    await Task.Delay(100);
                }
                catch (Exception ex)
                {
                    logger.LogInfo(sessionId, $"ERROR: [{ex.Message}]");
                    break;
                }
            }
            
            sessionService.RemoveRoom(sessionId);
        });
    }
}