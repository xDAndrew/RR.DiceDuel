using System.Collections.Concurrent;
using Microsoft.AspNetCore.SignalR;
using RR.DiceDuel.Core.Services.PlayerService.Models;
using RR.DiceDuel.ExternalServices.SignalR;

namespace RR.DiceDuel.Core.Services.PlayerService;

public class PlayerService(IHubContext<GameHub> hubContext) : IPlayerService
{
    private readonly ConcurrentDictionary<string, Player> _players = new();
    private readonly IHubContext<GameHub> _hubContext = hubContext;

    public void AddPlayer(string connectionId, string playerName)
    {
        _players.TryAdd(connectionId, new Player
        {
            Id = connectionId, Name = playerName, PlayerRoom = "Lobby"
        });
    }

    public void RemovePlayer(string connectionId)
    {
        var player = GetPlayer(connectionId);
        if (player != null)
        {
            _players.Remove(connectionId, out _);
        }
    }

    public void MoveToRoom(string connectionId, string roomId)
    {
        throw new NotImplementedException();
    }

    public List<Player> GetPlayers()
    {
        return _players.Select(x => x.Value).ToList();
    }

    private Player GetPlayer(string key)
    {
        var isPlayerExist = _players.TryGetValue(key, out var player);
        return isPlayerExist ? player : null;
    }
}