using System.Configuration;

namespace BasicAuthForMVC4.Common.Configuration
{
    public class ConfigurationManagerAdapter : IConfigurationManager
    {
        public string GetAppSetting(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }
    }
}
