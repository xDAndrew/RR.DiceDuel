using System.Collections.Concurrent;
using RR.DiceDuel.Core.Process.GameLoop;
using RR.DiceDuel.Core.Services.ConfigurationSerivce;
using RR.DiceDuel.Core.Services.PlayerService;
using RR.DiceDuel.Core.Services.SessionService.Models;

namespace RR.DiceDuel.Core.Services.SessionService;

public class SessionService(IPlayerService playerService, IServiceScopeFactory scopeFactory, 
    IConfigurationService configurationService) : ISessionService
{
    private readonly ConcurrentDictionary<string, Session> _session = new();
    
    public string GetOrCreateSession()
    {
        var config = configurationService.GetConfiguration();
        
        // Find empty room
        var sessions = GetSessions();
        var emptyRoom = sessions.FirstOrDefault(x => x.Players.Count < config.RoomMaxPlayer);
        if (emptyRoom != null)
        {
            return emptyRoom.SessionId;
        }

        // Create new room
        var newSessionId = Guid.NewGuid().ToString();
        var newSession = new Session
        {
            SessionId = newSessionId,
            Players = [],
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
        session?.Players.Add(player);
    }

    public void LeaveRoom(string sessionId, string playerId)
    {
        var player = playerService.GetPlayer(playerId);
        if (player is null)
        {
            return;
        }
        
        var session = GetSession(sessionId);
        session?.Players.Remove(player);
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
}