using RR.DiceDuel.Core.Services.SessionService.Models;

namespace RR.DiceDuel.Core.StateMachine.Interfaces;

public abstract class GameState
{
    public abstract void UpdateState(Session sessionContext, ref GameState nextState);

    protected static bool IsAllPlayersConnect(Session session)
    {
        return session.GameConfig.RoomMaxPlayer == session.PlayerStatus.Count;
    }
}