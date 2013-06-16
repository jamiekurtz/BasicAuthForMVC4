namespace BasicAuthForMVC4.Common.Configuration
{
    public interface IConfigurationManager
    {
        string GetAppSetting(string key);
    }
}