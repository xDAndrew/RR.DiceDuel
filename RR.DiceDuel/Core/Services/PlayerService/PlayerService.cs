using System.Collections.Concurrent;
using RR.DiceDuel.Core.Services.PlayerService.Models;

namespace RR.DiceDuel.Core.Services.PlayerService;

public class PlayerService : IPlayerService
{
    private static readonly ConcurrentDictionary<string, Player> PlayersCollection = new();

    public void AddPlayer(string connectionId, string playerName, string roomId)
    {
        Console.WriteLine($"AddPlayer id: [{connectionId}] name: [{playerName}]; room: [{roomId}]");
        PlayersCollection.TryAdd(connectionId, new Player
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
            PlayersCollection.Remove(connectionId, out _);
        }
    }

    public List<Player> GetPlayers()
    {
        return PlayersCollection.Select(x => x.Value).ToList();
    }

    public Player GetPlayer(string key)
    {
        var isPlayerExist = PlayersCollection.TryGetValue(key, out var player);
        return isPlayerExist ? player : null;
    }
}