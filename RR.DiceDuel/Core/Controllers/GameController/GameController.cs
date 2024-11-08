using RR.DiceDuel.Core.Services.ConfigurationSerivce;
using RR.DiceDuel.Core.Services.SessionService;
using RR.DiceDuel.Core.Services.SessionService.Types;
using RR.DiceDuel.Core.Services.StatisticService.Models;

namespace RR.DiceDuel.Core.Controllers.GameController;

public class GameController(ISessionService sessionService, IConfigurationService configurationService) : IGameController
{
    public void SetSessionState(string sessionId, SessionStateType newState)
    {
        var session = sessionService.GetSession(sessionId);
        if (session is null)
        {
            return;
        }

        session.CurrentState = newState;
    }

    public bool IsRoomFull(string sessionId)
    {
        var session = sessionService.GetSession(sessionId);
        if (session is null)
        {
            return false;
        }

        return session.PlayerStatus.Count == configurationService.GetConfiguration().RoomMaxPlayer;
    }

    public bool IsAllPlayersReady(string sessionId)
    {
        var session = sessionService.GetSession(sessionId);
        return session is not null && session.PlayerStatus.All(x => x.IsPlayerReady);
    }

    public bool IsLastPlayer(string sessionId)
    {
        var session = sessionService.GetSession(sessionId);
        if (session is null)
        {
            return false;
        }

        return session.PlayerStatus.Where(x => !x.IsPlayerLost).ToList().Count == 1;
    }

    public void CleanUpSessionForNewGame(string sessionId)
    {
        var session = sessionService.GetSession(sessionId);
        if (session is null)
        {
            return;
        }
        
        session.CurrentPlayer = 0;
        session.CurrentRound = 0;
        foreach (var player in session.PlayerStatus)
        {
            player.IsPlayerReady = false;
            player.IsPlayerLost = false;
            player.GameStatistic = new Statistic();
            player.LastInput = null;
        }
    }

    public int GetCurrentRound(string sessionId)
    {
        return sessionService.GetSession(sessionId)?.CurrentRound ?? 0;
    }
}