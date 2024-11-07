using RR.DiceDuel.Core.Services.ConfigurationSerivce.Models;
using RR.DiceDuel.Core.Services.SessionService.Types;
using RR.DiceDuel.Core.Services.StatisticService;

namespace RR.DiceDuel.Core.Services.SessionService.Models;

public class Session
{
    public string SessionId { get; set; }

    public SessionStateType CurrentState { get; set; } = SessionStateType.Started;
    
    public int CurrentRound { get; set; }

    public string CurrentPlayerMove { get; set; }
    
    public List<SessionPlayerStatus> PlayerStatus { get; set; }
    
    public AppConfiguration GameConfig { get; set; }

    public Stack<string> GameLog { get; set; } = [];
    
    public List<Statistic> GameResults  { get; set; } = [];
}