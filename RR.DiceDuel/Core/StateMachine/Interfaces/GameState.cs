namespace RR.DiceDuel.Core.StateMachine.Interfaces;

public abstract class GameState
{
    public abstract GameState UpdateState(string sessionId, AsyncServiceScope scope);
}