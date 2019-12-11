﻿using CoreEngine.APIHandlers;
using GalaSoft.MvvmLight.Command;
using Mobile.Core.Engines.Services;
using Mobile.Core.Models.Core;
using Mobile.Core.Worker;
using System.Windows.Input;

namespace Mobile.Core.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly IMemberHandler _memberHandler;
        private readonly SettingService _settingService;
        private readonly IPlatformService _platformService;

        public string UserName { get; set; }
        public string Password { get; set; }

        public LoginViewModel(IMemberHandler memberHandler, IPlatformService platformService, SettingService settingService)
        {
            _memberHandler = memberHandler;
            _settingService = settingService;
            _platformService = platformService;

        }
        public override void OnAppear(params object[] args)
        {
            UserName = string.Empty;
            Password = string.Empty;
#if DEBUG
            UserName = "181909";
            Password = "123456";
#endif
        }

        public ICommand LoginCommand => new RelayCommand(LoginAction);

        private async void LoginAction()
        {
            if (string.IsNullOrWhiteSpace(UserName))
            {
                _dialog.ShowMessage("Error", "Invalid Username. Username can not be empty");
            }
            else if (string.IsNullOrWhiteSpace(Password))
            {
                _dialog.ShowMessage("Error", "Invalid Password. Password can not be empty");
            }
            else
            {
                IsBusy = true;
                var res = await _memberHandler.Login(UserName, Password);
                if (res != null && res.Success)
                {
                    var user = await _memberHandler.TouchLogin();
                    if (user != null)
                    {
                        AppService.CurrentUser = user;
                        _settingService.Token = res.Token;
                        _nav.Init<HomeViewModel>();
                        if (AppService.CurrentUser.Batch != null)
                        {
                            _platformService.SubsubcribeTopics(AppService.CurrentUser.Batch.Name);
                        }
                    }
                }
                IsBusy = false;
            }
        }
    }
}
