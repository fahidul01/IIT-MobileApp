using Blazor.Extensions.Storage.Interfaces;
using CoreEngine.Engine;
using Microsoft.AspNetCore.Components;
using System;

namespace IIT.Client.Services
{
    public class PreferenceEngineProvider : IPreferenceEngine
    {
        private readonly ILocalStorage _localStorage;

        public PreferenceEngineProvider(ILocalStorage localStorage)
        {
            _localStorage = localStorage;
        }

        public string GetSetting(string key, string value)
        {
            try
            {
                var res = _localStorage.GetItem<string>(key).Result;
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
            await _localStorage.SetItem<string>(key, value);
        }
    }
}
