using RR.DiceDuel.Core.Services.GameLogService;
using RR.DiceDuel.Core.Services.GameService;
using RR.DiceDuel.Core.Services.SessionService.Types;
using RR.DiceDuel.Core.StateMachine.Interfaces;

namespace RR.DiceDuel.Core.StateMachine.States;

public class WaitingState : GameState
{
    private int _stepsBeforeCloseSession = 3000; // 5 min (1 step - 100 ms)
    
    public override GameState UpdateState(string sessionId, AsyncServiceScope scope)
    {
        var gameController = scope.ServiceProvider.GetRequiredService<IGameService>();
        var gameLogger = scope.ServiceProvider.GetRequiredService<IGameLogService>();
        
        gameController.SetSessionState(sessionId, SessionStateType.Started);

        if (!gameController.IsRoomFull(sessionId))
        {
            _stepsBeforeCloseSession--;
            return null;
        }
        
        gameLogger.LogInfo(sessionId, "The room is full. We are waiting for confirmation about the start of the game.");

        if (_stepsBeforeCloseSession == 0)
        {
            return new FinishState();
        } 
        
        return new PrepareGameState();
    }
}