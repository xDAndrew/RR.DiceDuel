using RR.DiceDuel.Core.Services.GameLogService;
using RR.DiceDuel.Core.Services.GameService;
using RR.DiceDuel.Core.Services.SessionService;
using RR.DiceDuel.Core.Services.SessionService.Types;
using RR.DiceDuel.Core.StateMachine.Interfaces;

namespace RR.DiceDuel.Core.StateMachine.States;

public class ConfirmationState : GameState
{
    private int _stepsUntilStart = 50; //1 step -> 100 ms
    private int _timeSec = 5; //sec
    
    public override GameState UpdateState(string sessionId, AsyncServiceScope scope)
    {
        var gameController = scope.ServiceProvider.GetRequiredService<IGameService>();
        var gameLogger = scope.ServiceProvider.GetRequiredService<IGameLogService>();
        var sessionService = scope.ServiceProvider.GetRequiredService<ISessionService>();

        var session = sessionService.GetSession(sessionId);
        
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
        
            session.Timer = _timeSec;
            gameController.NotifyPlayers(session);
            
            if (_stepsUntilStart % 10 == 0)
            {
                _timeSec--;
                gameLogger.LogInfo(sessionId, $"The game will start in {_stepsUntilStart / 10} seconds");
            }
            
            _stepsUntilStart--;
        }
        else
        {
            _timeSec = 5;
            _stepsUntilStart = 50;
            
            session.Timer = -1;
            gameController.NotifyPlayers(session);
        }
        
        return null;
    }
}