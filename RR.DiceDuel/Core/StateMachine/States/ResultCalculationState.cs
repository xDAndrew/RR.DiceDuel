using RR.DiceDuel.Core.Services.SessionService.Models;
using RR.DiceDuel.Core.Services.SessionService.Types;
using RR.DiceDuel.Core.StateMachine.Interfaces;

namespace RR.DiceDuel.Core.StateMachine.States;

public class ResultCalculationState : GameState
{
    public override void UpdateState(Session sessionContext, ref GameState nextState)
    {
        sessionContext.CurrentState = SessionStateType.ResultCalculation;
        
        var winner = sessionContext
            .PlayerStatus.Select(x => new { x.PlayerInfo, x.GameStatistic.TotalScores })
            .OrderBy(x => x.TotalScores).First();
        
        sessionContext.GameLog.Push($"The Winner is {winner.PlayerInfo.Name} with {winner.TotalScores} scores");
        
        foreach (var player in sessionContext.PlayerStatus)
        {
            player.GameStatistic.GamesCount++;
            sessionContext.GameResults.Add(player.GameStatistic);
        }

        nextState = new PrepareGameState();
    }
}