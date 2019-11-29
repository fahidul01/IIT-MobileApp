using CoreEngine.APIHandlers;
using Mobile.Core.Models.Core;
using Mobile.Core.Worker;
using System;

namespace Mobile.Core.ViewModels
{
    public class SplashViewModel : BaseViewModel
    {
        private readonly HttpWorker _httpFactory;
        private readonly SettingService _settingServicce;
        private readonly IMemberHandler _memberHandler;

        public SplashViewModel(HttpWorker httpWorker, IMemberHandler memberHandler, SettingService settingService)
        {
            _httpFactory = httpWorker;
            _settingServicce = settingService;
            _memberHandler = memberHandler;
        }
        public async override void OnAppear(params object[] args)
        {
            if (string.IsNullOrWhiteSpace(_settingServicce.Token))
            {
                await _nav.NavigateTo<LoginViewModel>();
            }
            else
            {
                _httpFactory.LoggedIn(_settingServicce.Token);
                var res = await _memberHandler.TouchLogin();
                if (res == null)
                {
                    _settingServicce.Token = string.Empty;
                    _httpFactory.Logout();
                    await _nav.NavigateTo<LoginViewModel>();
                }
                else
                {
                    AppService.CurrentUser = res;
                    AppService.HasCRRole = res.IsCR;
                    await _nav.NavigateTo<HomeViewModel>();
                }
            }
        }
    }
}
