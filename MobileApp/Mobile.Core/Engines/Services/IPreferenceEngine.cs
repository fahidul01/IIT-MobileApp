using System;
using System.Collections.Generic;
using System.Text;

namespace Mobile.Core.Engines.Services
{
    public interface IPreferenceEngine
    {
        string GetSetting(string key, string value);
        void SetSetting(string key, string value);
    }
}
