using RR.DiceDuel.Core.Services.SessionService.Models;

namespace RR.DiceDuel.Core.StateMachine.Interfaces;

public interface IGameState
{
    bool UpdateState(Session sessionContext, out IGameState nextState, out string message);
}