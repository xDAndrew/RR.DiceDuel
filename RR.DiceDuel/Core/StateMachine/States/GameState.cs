using RR.DiceDuel.Core.Services.SessionService.Models;
using RR.DiceDuel.Core.Services.SessionService.Types;
using RR.DiceDuel.Core.StateMachine.Interfaces;

namespace RR.DiceDuel.Core.StateMachine.States;

public class GameState : IGameState
{
    private int _round = 0;
    private int _currentPlayerTurn = 0;
    
    public bool UpdateState(Session sessionContext, out IGameState nextState, out string message)
    {
        sessionContext.CurrentState = SessionStateType.GameOngoing;
        _round++;
        _currentPlayerTurn++;
        nextState = null;
        message = null;
        return false;
    }
}