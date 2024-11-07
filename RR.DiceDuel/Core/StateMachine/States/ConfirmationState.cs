using RR.DiceDuel.Core.Services.SessionService.Models;
using RR.DiceDuel.Core.Services.SessionService.Types;
using RR.DiceDuel.Core.StateMachine.Interfaces;

namespace RR.DiceDuel.Core.StateMachine.States;

public class ConfirmationState : GameState
{
    private int _timeUntilStartSec = 50; 
    
    public override void UpdateState(Session sessionContext, ref GameState nextState)
    {
        if (!IsAllPlayersConnect(sessionContext))
        {
            nextState = new FinishState();
            return;
        }
        
        sessionContext.CurrentState = SessionStateType.WaitingConfirmation;
        if (sessionContext.PlayerStatus.All(x => x.IsPlayerReady))
        {
            if (_timeUntilStartSec == 0)
            {
                nextState = new GameOngoingState();
            }

            if (_timeUntilStartSec % 10 == 0)
            {
                sessionContext.GameLog.Push($"The game will start in {_timeUntilStartSec / 10} seconds");
            }
            
            _timeUntilStartSec--;
        }
        else
        {
            _timeUntilStartSec = 50;
        }
    }
}