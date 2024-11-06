using Microsoft.AspNetCore.SignalR;
using RR.DiceDuel.Core.Services.PlayerService;
using RR.DiceDuel.Core.Services.SessionService;

namespace RR.DiceDuel.ExternalServices.SignalR;

public class GameHub(IPlayerService playerService, ISessionService sessionService) : Hub
{
    public override async Task OnConnectedAsync()
    {
        var roomId = Context.GetHttpContext()?.Request.Query["roomId"];
        await Groups.AddToGroupAsync(Context.ConnectionId, roomId ?? "Lobby");
        playerService.AddPlayer(Context.ConnectionId, Context.User?.Identity?.Name, roomId);
        sessionService.JoinRoom(roomId, Context.ConnectionId);
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception exception)
    {
        var roomId = Context.GetHttpContext()?.Request.Query["roomId"];
        sessionService.LeaveRoom(roomId, Context.ConnectionId);
        playerService.RemovePlayer(Context.ConnectionId);
        await base.OnDisconnectedAsync(exception);
    }
    
    public async Task SendTestMessage(string roomId, string message)
    {
        await Clients.Group(roomId).SendAsync("Test", message);
    }
}