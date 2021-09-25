using Common.NetworkUtils.Interface;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

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
