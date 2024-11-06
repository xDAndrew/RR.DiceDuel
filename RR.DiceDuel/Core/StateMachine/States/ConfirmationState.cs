using RR.DiceDuel.Core.Services.SessionService.Models;
using RR.DiceDuel.Core.Services.SessionService.Types;
using RR.DiceDuel.Core.StateMachine.Interfaces;

namespace RR.DiceDuel.Core.StateMachine.States;

public class ConfirmationState : IGameState
{
    public bool UpdateState(Session sessionContext, out IGameState nextState, out string message)
    {
        sessionContext.CurrentState = SessionStateType.WaitingConfirmation;
        if (sessionContext.Players.All(x => x.IsPlayerReadyToPlay))
        {
            message = null;
            nextState = null;
            return true;
        }

        message = null;
        nextState = null;
        return false;
    }
}