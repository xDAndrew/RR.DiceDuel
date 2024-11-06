using RR.DiceDuel.Core.Services.SessionService.Models;
using RR.DiceDuel.Core.StateMachine.Interfaces;

namespace RR.DiceDuel.Core.StateMachine.States;

public class WaitingState : IGameState
{
    public bool UpdateState(Session sessionContext, out IGameState nextState, out string message)
    {
        if (sessionContext.Players.Count == sessionContext.GameConfig.RoomMaxPlayer)
        {
            message = "Waiting is full";
            nextState = new ConfirmationState();
            return true;
        }

        message = null;
        nextState = null;
        return false;
    }
}