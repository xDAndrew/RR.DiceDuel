using RR.DiceDuel.Core.Services.ConfigurationSerivce.Models;

namespace RR.DiceDuel.Core.Services.ConfigurationSerivce;

public interface IConfigurationService
{
    void SetConfiguration(AppConfiguration configuration);
    
    AppConfiguration GetConfiguration();
}