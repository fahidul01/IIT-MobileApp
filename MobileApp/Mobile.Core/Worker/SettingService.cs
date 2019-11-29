using Mobile.Core.Engines.Services;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Mobile.Core.Worker
{
    public class SettingService
    {
        private readonly IPreferenceEngine _preference;

        public SettingService(IPreferenceEngine preferenceEngine)
        {
            _preference = preferenceEngine;
        }

        public string Token
        {
            get => _preference.GetSetting(CallName(), "");
            set => _preference.SetSetting(CallName(), value);
        }

        private string CallName([CallerMemberName] string name = "")
        {
            return name;
        }
    }
}
