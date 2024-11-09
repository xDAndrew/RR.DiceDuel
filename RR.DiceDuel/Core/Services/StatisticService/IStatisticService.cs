using RR.DiceDuel.Core.Services.StatisticService.Models;

namespace RR.DiceDuel.Core.Services.StatisticService;

public interface IStatisticService
{
    void CreateOrUpdateStatistic(string userName, Statistic statistic);

    Statistic GetUserStatistic(string userName);

    Dictionary<string, Statistic> GetLeaders();
}