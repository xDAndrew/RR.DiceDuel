using System.Text.Json;
using Microsoft.AspNetCore.SignalR;
using RR.DiceDuel.Core.Services.ConfigurationSerivce;
using RR.DiceDuel.Core.Services.SessionService;
using RR.DiceDuel.Core.Services.SessionService.Models;
using RR.DiceDuel.ExternalServices.SignalR;

namespace RR.DiceDuel.Core.Controllers.PlayerController;

public class PlayerController(ISessionService sessionService, IConfigurationService configurationService, 
    IHubContext<GameHub> gameHub) : IPlayerController
{
    private readonly Random _random = new(Guid.NewGuid().GetHashCode());
    
    public void SetPlayerReady(string sessionId, string playerName)
    {
        var session = sessionService.GetSession(sessionId);

        var player = session?.PlayerStatus.FirstOrDefault(x => x.PlayerInfo.Name == playerName);
        if (player is null)
        {
            return;
        }

        NotifyPlayers(session);
        
        player.IsPlayerReady = !player.IsPlayerReady;
    }

    public List<int> SetRoll(string sessionId)
    {
        var session = sessionService.GetSession(sessionId);
        
        var result = new List<int>
        {
            _random.Next(1, 7),
            _random.Next(1, 7),
            _random.Next(1,7)
        };

        var currentPlayerIndex = session.CurrentPlayer;

        session.PlayerStatus[currentPlayerIndex].GameStatistic.NormalRolled++;
        session.PlayerStatus[currentPlayerIndex].GameStatistic.TotalScores += result.Sum();

        session.CurrentPlayer++;
        session.CurrentPlayer %= configurationService.GetConfiguration().RoomMaxPlayer;
        if (session.CurrentPlayer == 0)
        {
            session.CurrentRound++;
        }
        
        NotifyPlayers(session);
        
        return result;
    }

    public int SetSpecialRoll(string sessionId)
    {
        var session = sessionService.GetSession(sessionId);
        var roll = _random.Next(1, 7);

        var currentPlayerIndex = session.CurrentPlayer;

        switch (roll)
        {
            case 1:
                session.PlayerStatus[currentPlayerIndex].IsPlayerLost = true;
                session.PlayerStatus[currentPlayerIndex].GameStatistic.GotZeroScore++;
                break;
            case 6:
                roll = 24;
                session.PlayerStatus[currentPlayerIndex].GameStatistic.GotMaxScore++;
                break;
        }

        session.PlayerStatus[currentPlayerIndex].GameStatistic.SpecialRolled++;
        session.PlayerStatus[currentPlayerIndex].GameStatistic.TotalScores += roll;

        session.CurrentPlayer++;
        session.CurrentPlayer %= configurationService.GetConfiguration().RoomMaxPlayer;
        if (session.CurrentPlayer == 0)
        {
            session.CurrentRound++;
        }

        NotifyPlayers(session);
        
        return roll;
    }

    public string CurrentPlayerMoveName(string sessionId)
    {
        var session = sessionService.GetSession(sessionId);
        return session.PlayerStatus[session.CurrentPlayer].PlayerInfo.Name;
    }

    private void NotifyPlayers(Session session)
    {
        var json = JsonSerializer.Serialize(session);
        gameHub.Clients.Groups(session.SessionId).SendAsync("UpdateSession", json);
    }
}