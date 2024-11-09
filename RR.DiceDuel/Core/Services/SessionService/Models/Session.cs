using RR.DiceDuel.Core.Services.SessionService.Types;

namespace RR.DiceDuel.Core.Services.SessionService.Models;

public class Session
{
    public string SessionId { get; set; }

    public SessionStateType CurrentState { get; set; } = SessionStateType.Started;
    
    public int CurrentRound { get; set; }

    public int CurrentPlayer { get; set; }

    public int Timer { get; set; }

    public List<SessionPlayerStatus> PlayerStatus { get; set; }
}