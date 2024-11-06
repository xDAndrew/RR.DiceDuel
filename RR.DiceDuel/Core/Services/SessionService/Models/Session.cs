using RR.DiceDuel.Core.Services.ConfigurationSerivce.Models;
using RR.DiceDuel.Core.Services.PlayerService.Models;
using RR.DiceDuel.Core.Services.SessionService.Types;

namespace RR.DiceDuel.Core.Services.SessionService.Models;

public class Session
{
    public string SessionId { get; set; }

    public SessionStateType CurrentState = SessionStateType.Started;
    
    public List<Player> Players { get; set; }
    
    public AppConfiguration GameConfig { get; set; }
}