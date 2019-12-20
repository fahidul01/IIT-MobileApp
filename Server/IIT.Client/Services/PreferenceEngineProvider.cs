using Blazored.LocalStorage;
using CoreEngine.Engine;
using System;
using System.Threading.Tasks;

namespace IIT.Client.Services
{
    public class PreferenceEngineProvider : IPreferenceEngine
    {
        private readonly ILocalStorageService _localStorage;

        public PreferenceEngineProvider(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
        }

        public string GetSetting(string key, string value)
        {
            try
            {
                var res = Task.Run(()=>GetResult(key,value)).Result;
                if (string.IsNullOrWhiteSpace(res)) return value;
                else return res;
            }
            catch (Exception ex)
            {
                LogEngine.Error(ex);
                return value;
            }
        }

        public async void SetSetting(string key, string value)
        {
            await _localStorage.SetItemAsync(key, value);
        }

        private async Task<string> GetResult(string key, string def)
        {
            if (await _localStorage.ContainKeyAsync(key))
                return await _localStorage.GetItemAsync<string>(key);
            else return def;
        }
    }
}
