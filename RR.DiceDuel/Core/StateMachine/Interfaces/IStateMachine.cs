namespace RR.DiceDuel.Core.StateMachine.Interfaces;

public interface IStateMachine
{
    bool Update(string sessionId, AsyncServiceScope scope);
}