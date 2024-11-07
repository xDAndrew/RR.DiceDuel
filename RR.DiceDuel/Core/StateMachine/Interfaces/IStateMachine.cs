using RR.DiceDuel.Core.Services.SessionService.Models;

namespace RR.DiceDuel.Core.StateMachine.Interfaces;

public interface IStateMachine
{
    bool Update(Session gameContext);
}