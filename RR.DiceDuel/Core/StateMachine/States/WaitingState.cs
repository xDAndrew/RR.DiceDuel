using RR.DiceDuel.Core.Services.SessionService.Models;
using RR.DiceDuel.Core.Services.SessionService.Types;
using RR.DiceDuel.Core.StateMachine.Interfaces;

namespace RR.DiceDuel.Core.StateMachine.States;

public class WaitingState : GameState
{
    public override void UpdateState(Session sessionContext, ref GameState nextState)
    {
        sessionContext.CurrentState = SessionStateType.Started;
        
        if (!IsAllPlayersConnect(sessionContext))
        {
            return;
        }
        
        sessionContext.GameLog.Push("The room is full. We are waiting for confirmation about the start of the game.");
        nextState = new PrepareGameState();
    }
}