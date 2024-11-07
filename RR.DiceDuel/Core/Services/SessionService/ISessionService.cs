using RR.DiceDuel.Core.Services.SessionService.Models;

namespace RR.DiceDuel.Core.Services.SessionService;

public interface ISessionService
{
    string GetOrCreateSession();

    void JoinRoom(string sessionId, string player);
    
    void LeaveRoom(string sessionId, string player);
    
    List<Session> GetSessions();

    void SetPlayerReady(string sessionId, string playerName);
    
    void SetPlayerMove(string sessionId, string playerName, string move);
}