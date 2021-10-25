using Common.NetworkUtils.Interfaces;
using System;
using System.Configuration;
using System.Collections.Specialized;

namespace Common.NetworkUtils
{
    public class SettingsManager : ISettingsManager
    {
        public string ReadSetting(string key)
        {
            try
            {
                NameValueCollection appSettings = ConfigurationManager.AppSettings;
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
