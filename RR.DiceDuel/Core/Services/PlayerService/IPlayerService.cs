using RR.DiceDuel.Core.Services.PlayerService.Models;

namespace RR.DiceDuel.Core.Services.PlayerService;

public interface IPlayerService
{
    void AddPlayer(string connectionId, string playerName);
    
    void RemovePlayer(string connectionId);

    void MoveToRoom(string connectionId, string roomId);

    List<Player> GetPlayers();
}