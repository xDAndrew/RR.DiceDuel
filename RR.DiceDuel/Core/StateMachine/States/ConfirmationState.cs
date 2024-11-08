using RR.DiceDuel.Core.Controllers.GameController;
using RR.DiceDuel.Core.Services.GameLogService;
using RR.DiceDuel.Core.Services.SessionService.Types;
using RR.DiceDuel.Core.StateMachine.Interfaces;

namespace RR.DiceDuel.Core.StateMachine.States;

public class ConfirmationState : GameState
{
    private int _stepsUntilStart = 50; //1 step -> 100 ms
    
    public override GameState UpdateState(string sessionId, AsyncServiceScope scope)
    {
        var gameController = scope.ServiceProvider.GetRequiredService<IGameController>();
        var gameLogger = scope.ServiceProvider.GetRequiredService<IGameLogService>();
        
        if (!gameController.IsRoomFull(sessionId))
        {
            return new WaitingState();
        }
        
        gameController.SetSessionState(sessionId, SessionStateType.WaitingConfirmation);
        
        if (gameController.IsAllPlayersReady(sessionId))
        {
            if (_stepsUntilStart == 0)
            {
                return new GameOngoingState();
            }
        
            if (_stepsUntilStart % 10 == 0)
            {
                gameLogger.LogInfo(sessionId, $"The game will start in {_stepsUntilStart / 10} seconds");
            }
            
            _stepsUntilStart--;
        }
        
        _stepsUntilStart = 50;
        return null;
    }
}