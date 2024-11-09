namespace RR.DiceDuel.Core.Services.AuthService.Types;

public enum AuthStatusType
{
    Success,
    UserAlreadyExist,
    UserNotFound,
    WrongPassword,
    UserAlreadyConnected
}