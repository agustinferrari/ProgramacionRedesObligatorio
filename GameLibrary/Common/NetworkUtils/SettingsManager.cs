using Common.NetworkUtils.Interfaces;
using System;
using System.Configuration;

namespace Common.NetworkUtils
{
    public class SettingsManager : ISettingsManager
    {
        public string ReadSetting(string key)
        {
            try
            {
                var appSettings = ConfigurationManager.AppSettings;
                return appSettings[key] ?? string.Empty;
            }
            catch (ConfigurationErrorsException)
            {
                Console.WriteLine("Error reading app settings");
                return string.Empty;
            }
        }
    }
}
