using System.Collections.Concurrent;
using RR.DiceDuel.Core.Process;
using RR.DiceDuel.Core.Services.ConfigurationSerivce;
using RR.DiceDuel.Core.Services.PlayerService;
using RR.DiceDuel.Core.Services.SessionService.Models;
using RR.DiceDuel.Core.Services.SessionService.Types;
using RR.DiceDuel.Core.Services.StatisticService;

namespace RR.DiceDuel.Core.Services.SessionService;

public class SessionService(IPlayerService playerService, IServiceScopeFactory scopeFactory, 
    IConfigurationService configurationService) : ISessionService
{
    private readonly ConcurrentDictionary<string, Session> _session = new();
    
    public string GetOrCreateSession()
    {
        var config = configurationService.GetConfiguration();
        
        // Clean up sessions data
        CleanUpSessions();
        
        // Find empty room
        var sessions = GetSessions();
        var emptyRoom = sessions.FirstOrDefault(x => x.PlayerStatus.Count < config.RoomMaxPlayer 
            && x.CurrentState == SessionStateType.Started);
        if (emptyRoom != null)
        {
            return emptyRoom.SessionId;
        }

        // Create new room
        var newSessionId = Guid.NewGuid().ToString();
        var newSession = new Session
        {
            SessionId = newSessionId,
            PlayerStatus = [],
            GameConfig = config
        };
        _session.TryAdd(newSessionId, newSession);

        var gameLoop = new GameLoop(scopeFactory, newSession);
        gameLoop.Invoke();
        
        return newSessionId;
    }

    public void JoinRoom(string sessionId, string playerId)
    {
        var player = playerService.GetPlayer(playerId);
        if (player is null)
        {
            return;
        }
        
        var session = GetSession(sessionId);
        var playerStatus = new SessionPlayerStatus
        {
            PlayerInfo = player,
            GameStatistic = new Statistic()
        };
        
        session?.PlayerStatus.Add(playerStatus);
    }

    public void LeaveRoom(string sessionId, string playerId)
    {
        var player = playerService.GetPlayer(playerId);
        if (player is null)
        {
            return;
        }
        
        var session = GetSession(sessionId);
        var playerStatus = session?.PlayerStatus?.FirstOrDefault(x => x.PlayerInfo.Id == player.Id);
        if (playerStatus != null)
        {
            session.PlayerStatus.Remove(playerStatus);
        }
    }
    
    private Session GetSession(string key)
    {
        var isSessionExist = _session.TryGetValue(key, out var session);
        return isSessionExist ? session : null;
    }
    
    public List<Session> GetSessions()
    {
        return _session.Select(x => x.Value).ToList();
    }

    public void SetPlayerReady(string sessionId, string playerName)
    {
        var session = GetSession(sessionId);
        if (session is null)
        {
            return;
        }

        var player = session.PlayerStatus.FirstOrDefault(x => x.PlayerInfo.Name == playerName);
        if (player is null)
        {
            return;
        }

        player.IsPlayerReady = !player.IsPlayerReady;
    }

    public void SetPlayerMove(string sessionId, string playerName, string move)
    {
        var session = GetSession(sessionId);
        if (session is null)
        {
            return;
        }

        var player = session.PlayerStatus.FirstOrDefault(x => x.PlayerInfo.Name == playerName);
        if (player is null)
        {
            return;
        }

        player.LastInput = move;
    }

    private void CleanUpSessions()
    {
        var sessions = _session.Where(x => x.Value.CurrentState == SessionStateType.Finish);
        foreach (var session in sessions)
        {
            _session.TryRemove(session.Key, out _);
        }
    }
}