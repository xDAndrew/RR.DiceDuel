using Microsoft.Extensions.Caching.Memory;
using RR.DiceDuel.Core.Services.StatisticService.Models;
using RR.DiceDuel.ExternalServices.EntityFramework;
using RR.DiceDuel.ExternalServices.EntityFramework.Entities;

namespace RR.DiceDuel.Core.Services.StatisticService;

public class StatisticService(GameContext dbContext, IMemoryCache memoryCache) : IStatisticService
{
    private const string LeadersCacheKey = "GameLeaders";
    
    public void CreateOrUpdateStatistic(string userName, Statistic statistic)
    {
        var leaders = memoryCache.Get<Dictionary<string, Statistic>>(LeadersCacheKey);
        if (leaders is not null && leaders.Count > 0)
        {
            var minScore = leaders.Min(x => x.Value.TotalScores);
            if (statistic.TotalScores > minScore || leaders.ContainsKey(userName))
            {
                memoryCache.Remove(LeadersCacheKey);
            }
        }

        var userStatistic = dbContext.Statistics.FirstOrDefault(x => x.PlayerName == userName);
        if (userStatistic is null)
        {
            dbContext.Statistics.Add(new StatisticEntity
            {
                PlayerName = userName,
                Wins = statistic.Wins,
                Draw = statistic.Draw,
                Defeats = statistic.Defeats,
                GamesCount = statistic.GamesCount,
                TotalScores = statistic.TotalScores,
                GotMaxScore = statistic.GotMaxScore,
                NormalRolled = statistic.NormalRolled,
                SpecialRolled = statistic.SpecialRolled,
                GotZeroScore = statistic.GotZeroScore
            });
            dbContext.SaveChanges();
            return;
        }

        userStatistic.Wins += statistic.Wins;
        userStatistic.Draw += statistic.Draw;
        userStatistic.Defeats += statistic.Defeats;
        userStatistic.GamesCount += statistic.GamesCount;
        userStatistic.TotalScores += statistic.TotalScores;
        userStatistic.GotMaxScore += statistic.GotMaxScore;
        userStatistic.NormalRolled += statistic.NormalRolled;
        userStatistic.SpecialRolled += statistic.SpecialRolled;
        userStatistic.GotZeroScore += statistic.GotZeroScore;
        dbContext.SaveChanges();
    }

    public Statistic GetUserStatistic(string userName)
    {
        var statistic = dbContext.Statistics.FirstOrDefault(x => x.PlayerName == userName);
        return statistic is null ? new Statistic() : MapEntityToDto(statistic);
    }

    public Dictionary<string, Statistic> GetLeaders()
    {
        var leaders = dbContext.Statistics.Take(10).OrderBy(x => x.TotalScores).ToList();
        if (leaders.Count == 0)
        {
            return new Dictionary<string, Statistic>();
        }
        
        var result = leaders.Select(x => 
            new KeyValuePair<string, Statistic>(x.PlayerName, MapEntityToDto(x))).ToDictionary();

        memoryCache.Set(LeadersCacheKey, result);
        return result;
    }

    private static Statistic MapEntityToDto(StatisticEntity entity)
    {
        return new Statistic
        {
            Defeats = entity.Defeats,
            Draw = entity.Draw,
            Wins = entity.Wins,
            GamesCount = entity.GamesCount,
            NormalRolled = entity.NormalRolled,
            SpecialRolled = entity.SpecialRolled,
            TotalScores = entity.TotalScores,
            GotMaxScore = entity.GotMaxScore,
            GotZeroScore = entity.GotZeroScore
        };
    }
}