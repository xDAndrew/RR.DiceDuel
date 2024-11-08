using RR.DiceDuel.Core.Services.SessionService.Types;

namespace RR.DiceDuel.Core.Controllers.GameController;

public interface IGameController
{
    void SetSessionState(string sessionId, SessionStateType newState);

    bool IsRoomFull(string sessionId);

    bool IsAllPlayersReady(string sessionId);

    bool IsLastPlayer(string sessionId);

    void CleanUpSessionForNewGame(string sessionId);

    int GetCurrentRound(string sessionId);
}