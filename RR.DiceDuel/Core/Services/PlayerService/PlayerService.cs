using System.Collections.Concurrent;
using RR.DiceDuel.Core.Services.PlayerService.Models;

namespace RR.DiceDuel.Core.Services.PlayerService;

public class PlayerService : IPlayerService
{
    private readonly ConcurrentDictionary<string, Player> _players = new();

    public void AddPlayer(string connectionId, string playerName, string roomId)
    {
        Console.WriteLine($"AddPlayer id: [{connectionId}] name: [{playerName}]; room: [{roomId}]");
        _players.TryAdd(connectionId, new Player
        {
            Id = connectionId, Name = playerName, PlayerRoom = roomId
        });
    }

    public void RemovePlayer(string connectionId)
    {
        Console.WriteLine($"RemovePlayer [{connectionId}]");
        var player = GetPlayer(connectionId);
        if (player != null)
        {
            _players.Remove(connectionId, out _);
        }
    }

    public void MoveToRoom(string connectionId, string roomId)
    {
        var player = GetPlayer(connectionId);
        if (player != null)
        {
            player.PlayerRoom = roomId;
        }
    }

    public List<Player> GetPlayers()
    {
        return _players.Select(x => x.Value).ToList();
    }

    public Player GetPlayer(string key)
    {
        var isPlayerExist = _players.TryGetValue(key, out var player);
        return isPlayerExist ? player : null;
    }
}