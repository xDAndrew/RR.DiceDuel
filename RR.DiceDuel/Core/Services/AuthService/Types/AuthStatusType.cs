﻿namespace RR.DiceDuel.Core.Services.AuthService.Types;

public enum AuthStatusType
{
    SUCCESS,
    USER_ALREADY_EXIST,
    USER_NOT_FOUND,
    WRONG_PASSWORD,
    USER_ALREADY_CONNECTED
}