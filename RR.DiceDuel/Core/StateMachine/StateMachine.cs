using RR.DiceDuel.Core.StateMachine.Interfaces;
using RR.DiceDuel.Core.StateMachine.States;

namespace RR.DiceDuel.Core.StateMachine;

public class StateMachine : IStateMachine
{
    private GameState _currentState = new WaitingState();
    
    public bool Update(string sessionId, AsyncServiceScope scope)
    {
        var nextState = _currentState.UpdateState(sessionId, scope);
        if (nextState != null)
        {
            _currentState = nextState;
        }
        
        return _currentState is FinishState;
    }
}