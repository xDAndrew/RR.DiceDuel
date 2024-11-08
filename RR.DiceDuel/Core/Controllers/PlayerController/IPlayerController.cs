namespace RR.DiceDuel.Core.Controllers.PlayerController;

public interface IPlayerController
{
    void SetPlayerReady(string sessionId, string playerName);
    
    List<int> SetRoll(string sessionId);

    int SetSpecialRoll(string sessionId);

    string CurrentPlayerMoveName(string sessionId);
}