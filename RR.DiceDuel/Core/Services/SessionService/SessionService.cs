using System.Collections.Concurrent;
using RR.DiceDuel.Core.Services.ConfigurationSerivce;
using RR.DiceDuel.Core.Services.PlayerService;
using RR.DiceDuel.Core.Services.SessionService.Models;
using RR.DiceDuel.Core.Services.SessionService.Types;
using RR.DiceDuel.Core.Services.StatisticService.Models;
using RR.DiceDuel.Core.StateMachine;

namespace RR.DiceDuel.Core.Services.SessionService;

public class SessionService(IPlayerService playerService, IServiceScopeFactory scopeFactory, 
    IConfigurationService configurationService) : ISessionService
{
    private static readonly ConcurrentDictionary<string, Session> SessionCollection = new();
    
    public string GetOrCreateSession()
    {
        var config = configurationService.GetConfiguration();

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
            PlayerStatus = []
        };
        SessionCollection.TryAdd(newSessionId, newSession);

        var gameLoop = new GameLoop(scopeFactory, newSession.SessionId);
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
    
    public Session GetSession(string key)
    {
        var isSessionExist = SessionCollection.TryGetValue(key, out var session);
        return isSessionExist ? session : null;
    }

    public void RemoveRoom(string sessionId)
    {
        SessionCollection.TryRemove(sessionId, out _);
    }

    public List<Session> GetSessions()
    {
        return SessionCollection.Select(x => x.Value).ToList();
    }
}