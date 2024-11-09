using RR.DiceDuel.Core.Services.SessionService;
using RR.DiceDuel.Core.Services.SessionService.Models;
using RR.DiceDuel.Core.Services.SessionService.Types;
using RR.DiceDuel.Core.Services.StatisticService;
using RR.DiceDuel.Core.StateMachine.Interfaces;

namespace RR.DiceDuel.Core.StateMachine.States;

public class ResultCalculationState : GameState
{
    public override GameState UpdateState(string sessionId, AsyncServiceScope scope)
    {
        var sessionService = scope.ServiceProvider.GetRequiredService<ISessionService>();
        var statisticService = scope.ServiceProvider.GetRequiredService<IStatisticService>();
        var session = sessionService.GetSession(sessionId);
        
        session.CurrentState = SessionStateType.ResultCalculation;

        var maxScore = session.PlayerStatus.Select(x => x.GameStatistic.TotalScores).Max();
        var winnersCount = session.PlayerStatus
            .Where(x => x.GameStatistic.TotalScores == maxScore && !x.IsPlayerLost).ToList().Count;

        CalculateWinner(session, maxScore, winnersCount == 1);
        
        foreach (var player in session.PlayerStatus)
        {
            statisticService.CreateOrUpdateStatistic(player.PlayerInfo.Name, player.GameStatistic);
        }
        
        return new GameResultState();
    }

    private static void CalculateWinner(Session session, long maxScore, bool isOneWinner)
    {
        foreach (var player in session.PlayerStatus)
        {
            player.GameStatistic.GamesCount++;
            player.IsPlayerReady = false;
            if (player.IsPlayerLost)
            {
                player.GameStatistic.Defeats++;
                continue;
            }
                
            if (player.GameStatistic.TotalScores == maxScore)
            {
                if (isOneWinner)
                {
                    player.GameStatistic.Wins++;
                }
                else
                {
                    player.GameStatistic.Draw++;
                }
            }
            else
            {
                player.GameStatistic.Defeats++;
            }
        }
    }
}