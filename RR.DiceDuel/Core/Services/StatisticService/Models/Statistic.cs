namespace RR.DiceDuel.Core.Services.StatisticService.Models;

public class Statistic
{
    public long GamesCount { get; set; }
    
    public long Wins { get; set; }
    
    public long Draw { get; set; }
    
    public long Defeats { get; set; }
    
    public long NormalRolled { get; set; }
    
    public long SpecialRolled { get; set; }
    
    public long GotZeroScore { get; set; }
    
    public long GotMaxScore { get; set; }
    
    public long TotalScores { get; set; }
}