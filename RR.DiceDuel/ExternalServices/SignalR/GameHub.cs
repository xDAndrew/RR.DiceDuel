using Microsoft.AspNetCore.SignalR;
using RR.DiceDuel.Core.Services.PlayerService;

namespace RR.DiceDuel.ExternalServices.SignalR;

public class GameHub(IPlayerService playerService) : Hub
{
    public override async Task OnConnectedAsync()
    {
        await base.OnConnectedAsync();
        playerService.AddPlayer(Context.ConnectionId, Context.User?.Identity?.Name);
    }

    public override async Task OnDisconnectedAsync(Exception exception)
    {
        await base.OnDisconnectedAsync(exception);
        playerService.RemovePlayer(Context.ConnectionId);
    }
}