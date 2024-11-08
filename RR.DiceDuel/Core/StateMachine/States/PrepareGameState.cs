using RR.DiceDuel.Core.Controllers.GameController;
using RR.DiceDuel.Core.StateMachine.Interfaces;

namespace RR.DiceDuel.Core.StateMachine.States;

public class PrepareGameState : GameState
{
    public override GameState UpdateState(string sessionId, AsyncServiceScope scope)
    {
        var gameController = scope.ServiceProvider.GetRequiredService<IGameController>();
        gameController.CleanUpSessionForNewGame(sessionId);
        return new ConfirmationState();
    }
}