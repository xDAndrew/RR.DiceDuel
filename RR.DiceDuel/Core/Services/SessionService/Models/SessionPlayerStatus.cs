﻿using RR.DiceDuel.Core.Services.PlayerService.Models;
using RR.DiceDuel.Core.Services.StatisticService.Models;

namespace RR.DiceDuel.Core.Services.SessionService.Models;

public class SessionPlayerStatus
{
    public Player PlayerInfo { get; set; } 
    
    public bool IsPlayerReady { get; set; }
    
    public bool IsPlayerLost { get; set; }
    
    public Statistic GameStatistic { get; set; } = new();
}