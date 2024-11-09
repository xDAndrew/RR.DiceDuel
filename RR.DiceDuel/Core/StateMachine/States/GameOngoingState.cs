using RR.DiceDuel.Core.Services.ConfigurationSerivce;
using RR.DiceDuel.Core.Services.GameLogService;
using RR.DiceDuel.Core.Services.GameService;
using RR.DiceDuel.Core.Services.SessionService.Types;
using RR.DiceDuel.Core.StateMachine.Interfaces;

namespace RR.DiceDuel.Core.StateMachine.States;

public class GameOngoingState : GameState
{
    public override GameState UpdateState(string sessionId, AsyncServiceScope scope)
    {
        var gameController = scope.ServiceProvider.GetRequiredService<IGameService>();
        var logger = scope.ServiceProvider.GetRequiredService<IGameLogService>();
        var gameConfig = scope.ServiceProvider.GetRequiredService<IConfigurationService>();
        
        gameController.SetSessionState(sessionId, SessionStateType.GameOngoing);
        
        if (!gameController.IsRoomFull(sessionId))
        {
            return new WaitingState();
        }

        // If there is 1 player left in the game
        if (gameController.IsLastPlayer(sessionId))
        {
            logger.LogInfo(sessionId, "This match is over");
            return new ResultCalculationState();
        }

        // Last round
        if (gameController.GetCurrentRound(sessionId) == gameConfig.GetConfiguration().MaxGameRound)
        {
            logger.LogInfo(sessionId, "This match is over");
            return new ResultCalculationState();
        }
        
        return null;
    }
}