using RR.DiceDuel.Core.Services.SessionService.Models;
using RR.DiceDuel.Core.Services.SessionService.Types;
using RR.DiceDuel.Core.StateMachine.Interfaces;

namespace RR.DiceDuel.Core.StateMachine.States;

public class FinishState : GameState
{
    public override void UpdateState(Session sessionContext, ref GameState nextState)
    {
        sessionContext.CurrentState = SessionStateType.Finish;
        sessionContext.GameLog.Push("Game Finished. Session closed.");
    }
}