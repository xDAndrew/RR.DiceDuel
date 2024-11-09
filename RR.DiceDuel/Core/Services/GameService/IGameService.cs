using RR.DiceDuel.Core.Services.SessionService.Models;
using RR.DiceDuel.Core.Services.SessionService.Types;

namespace RR.DiceDuel.Core.Services.GameService;

public interface IGameService
{
    void SetSessionState(string sessionId, SessionStateType newState);

    bool IsRoomFull(string sessionId);

    bool IsAllPlayersReady(string sessionId);

    bool IsLastPlayer(string sessionId);

    void CleanUpSessionForNewGame(string sessionId);

    int GetCurrentRound(string sessionId);

    void NotifyPlayers(Session session);
}