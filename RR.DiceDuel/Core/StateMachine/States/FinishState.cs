using RR.DiceDuel.Core.StateMachine.Interfaces;

namespace RR.DiceDuel.Core.StateMachine.States;

public class FinishState : GameState
{
    public override GameState UpdateState(string sessionId, AsyncServiceScope scope)
    {
        return null;
    }
}