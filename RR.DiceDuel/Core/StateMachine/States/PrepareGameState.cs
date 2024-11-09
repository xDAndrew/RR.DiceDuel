using RR.DiceDuel.Core.Services.GameService;
using RR.DiceDuel.Core.StateMachine.Interfaces;

namespace RR.DiceDuel.Core.StateMachine.States;

public class PrepareGameState : GameState
{
    public override GameState UpdateState(string sessionId, AsyncServiceScope scope)
    {
        var gameController = scope.ServiceProvider.GetRequiredService<IGameService>();
        gameController.CleanUpSessionForNewGame(sessionId);
        return new ConfirmationState();
    }
}