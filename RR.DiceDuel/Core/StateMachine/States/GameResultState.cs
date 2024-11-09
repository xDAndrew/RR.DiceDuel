using RR.DiceDuel.Core.Services.GameService;
using RR.DiceDuel.Core.Services.SessionService.Types;
using RR.DiceDuel.Core.StateMachine.Interfaces;

namespace RR.DiceDuel.Core.StateMachine.States;

public class GameResultState : GameState
{
    public override GameState UpdateState(string sessionId, AsyncServiceScope scope)
    {
        var gameController = scope.ServiceProvider.GetRequiredService<IGameService>();
        
        if (!gameController.IsRoomFull(sessionId))
        {
            return new WaitingState();
        }
        
        gameController.SetSessionState(sessionId, SessionStateType.ShowResult);
        
        return gameController.IsAllPlayersReady(sessionId) ? new PrepareGameState() : null;
    }
}