using Microsoft.Extensions.Caching.Memory;
using RR.DiceDuel.Core.Services.ConfigurationSerivce.Models;
using RR.DiceDuel.ExternalServices.EntityFramework;
using RR.DiceDuel.ExternalServices.EntityFramework.Entities;

namespace RR.DiceDuel.Core.Services.ConfigurationSerivce;

public class ConfigurationService(IMemoryCache memoryCache, GameContext gameContext) : IConfigurationService
{
    private const string CacheKey = "GameConfigurationCacheKey";
    
    public void SetConfiguration(AppConfiguration newConfig)
    {
        memoryCache.Remove(CacheKey);
        
        var currentConfig = gameContext.Config.FirstOrDefault();
        if (currentConfig is null)
        {
            gameContext.Config.Add(new ConfigEntity
            {
                MaxGameRound = newConfig.MaxGameRound,
                RoomMaxPlayer = newConfig.RoomMaxPlayer
            });
            gameContext.SaveChanges();
            return;
        }
        
        currentConfig.RoomMaxPlayer = newConfig.RoomMaxPlayer;
        currentConfig.MaxGameRound = newConfig.MaxGameRound;
        gameContext.SaveChanges();
    }

    public AppConfiguration GetConfiguration()
    {
        var isCacheConfigExist = memoryCache.TryGetValue(CacheKey, out var cacheData);
        if (isCacheConfigExist && cacheData is AppConfiguration data)
        {
            return data;
        }
        
        var currentConfig = gameContext.Config.FirstOrDefault();
        if (currentConfig is null)
        {
            return new AppConfiguration();
        }
        
        var result = new AppConfiguration
        {
            RoomMaxPlayer = currentConfig.RoomMaxPlayer,
            MaxGameRound = currentConfig.MaxGameRound
        };

        memoryCache.Set(CacheKey, result);
        return result;
    }
}