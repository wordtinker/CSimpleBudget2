using System.Configuration;

namespace SimpleBudget.Tools
{
    public static class Settings
    {
        public static string Read(string key)
        {
            try
            {
                var appSettings = ConfigurationManager.AppSettings;
                string result = appSettings[key] ?? string.Empty;
                return result;
            }
            catch (ConfigurationErrorsException)
            {
                return string.Empty;
            }
        }
    }
}