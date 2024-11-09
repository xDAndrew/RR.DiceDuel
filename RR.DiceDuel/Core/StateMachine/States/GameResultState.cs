using RR.DiceDuel.Core.Controllers.GameController;
using RR.DiceDuel.Core.Services.SessionService.Types;
using RR.DiceDuel.Core.StateMachine.Interfaces;

namespace RR.DiceDuel.Core.StateMachine.States;

public class GameResultState : GameState
{
    public override GameState UpdateState(string sessionId, AsyncServiceScope scope)
    {
        var gameController = scope.ServiceProvider.GetRequiredService<IGameController>();
        
        if (!gameController.IsRoomFull(sessionId))
        {
            return new WaitingState();
        }
        
        gameController.SetSessionState(sessionId, SessionStateType.ShowResult);
        
        return gameController.IsAllPlayersReady(sessionId) ? new PrepareGameState() : null;
    }
}