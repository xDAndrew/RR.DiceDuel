using RR.DiceDuel.Core.Services.SessionService.Models;
using RR.DiceDuel.Core.StateMachine.Interfaces;

namespace RR.DiceDuel.Core.StateMachine.States;

public class ResultCalculationState : IGameState
{
    public bool UpdateState(Session sessionContext, out IGameState nextState, out string message)
    {
        nextState = null;
        message = null;
        return false;
    }
}