using System.Text.Json;
using RR.DiceDuel.Core.Services.ConfigurationSerivce.Models;

namespace RR.DiceDuel.Core.Services.ConfigurationSerivce;

public class ConfigurationService : IConfigurationService
{
    public void SetConfiguration(AppConfiguration configuration)
    {
        var json = JsonSerializer.Serialize(configuration);
        File.WriteAllText("configuration.json", json);
    }

    public AppConfiguration GetConfiguration()
    {
        var json = File.ReadAllText("configuration.json");
        return JsonSerializer.Deserialize<AppConfiguration>(json);
    }
}