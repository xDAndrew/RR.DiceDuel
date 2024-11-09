namespace RR.DiceDuel.Core.Services.SessionService.Types;

public enum SessionStateType
{
    Started, 
    WaitingConfirmation, 
    GameOngoing, 
    ResultCalculation,
    ShowResult,
    Finish
}