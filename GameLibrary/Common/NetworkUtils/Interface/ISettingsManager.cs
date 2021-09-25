using System;
using System.Collections.Generic;
using System.Text;

namespace Common.NetworkUtils.Interface
{
    public interface ISettingsManager
    {
        public string ReadSetting(string key);
    }
}
