namespace RR.DiceDuel.Core.Services.PlayerControllerService;

public interface IPlayerControllerService
{
    void SetPlayerReady(string sessionId, string playerName);
    
    List<int> SetRoll(string sessionId);

    int SetSpecialRoll(string sessionId);

    string CurrentPlayerMoveName(string sessionId);
}