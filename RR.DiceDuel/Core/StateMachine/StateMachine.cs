using System.Text.Json;
using Microsoft.AspNetCore.SignalR;
using RR.DiceDuel.Core.Services.SessionService.Models;
using RR.DiceDuel.Core.Services.SessionService.Types;
using RR.DiceDuel.Core.StateMachine.Interfaces;
using RR.DiceDuel.Core.StateMachine.States;
using RR.DiceDuel.ExternalServices.SignalR;

namespace RR.DiceDuel.Core.StateMachine;

public class StateMachine(IHubContext<GameHub> hubContext) : IStateMachine
{
    private GameState _currentState = new WaitingState();
    private GameState _nextState;
    
    public bool Update(Session gameContext)
    {
        _currentState.UpdateState(gameContext, ref _nextState);
        if (_nextState != null)
        {
            _currentState = _nextState;
            _nextState = null;
        }

        var json = JsonSerializer.Serialize(gameContext);
        hubContext.Clients.Groups(gameContext.SessionId).SendAsync("Test", json);

        if (gameContext.CurrentState != SessionStateType.Finish)
        {
            return true;
        }
        
        // Finish game
        hubContext.Clients.Groups(gameContext.SessionId).SendAsync("Test", "Game finished!");
        return false;
    }
}