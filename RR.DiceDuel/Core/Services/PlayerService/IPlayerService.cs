using RR.DiceDuel.Core.Services.PlayerService.Models;

namespace RR.DiceDuel.Core.Services.PlayerService;

public interface IPlayerService
{
    void AddPlayer(string connectionId, string playerName, string roomId);
    
    void RemovePlayer(string connectionId);

    List<Player> GetPlayers();

    Player GetPlayer(string connectionId);
}