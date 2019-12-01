using Mobile.Core.Engines.Services;
using Xamarin.Essentials;

namespace MobileApp.Service
{
    public class PreferenceEngine : IPreferenceEngine
    {
        public string GetSetting(string key, string value)
        {
            return Preferences.Get(key, value);
        }

        public void SetSetting(string key, string value)
        {
            Preferences.Set(key, value);
        }
    }
}
