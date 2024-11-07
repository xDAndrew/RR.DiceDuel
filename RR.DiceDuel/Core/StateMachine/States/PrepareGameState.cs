using RR.DiceDuel.Core.Services.SessionService.Models;
using RR.DiceDuel.Core.Services.StatisticService;
using RR.DiceDuel.Core.StateMachine.Interfaces;

namespace RR.DiceDuel.Core.StateMachine.States;

public class PrepareGameState : GameState
{
    public override void UpdateState(Session sessionContext, ref GameState nextState)
    {
        sessionContext.CurrentPlayerMove = null;
        sessionContext.CurrentRound = 0;
        foreach (var player in sessionContext.PlayerStatus)
        {
            player.IsPlayerReady = false;
            player.IsPlayerLost = false;
            player.GameStatistic = new Statistic();
            player.LastInput = null;
        }

        nextState = new ConfirmationState();
    }
}