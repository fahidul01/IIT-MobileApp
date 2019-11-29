using CoreEngine.APIHandlers;
using GalaSoft.MvvmLight.Command;
using Mobile.Core.Models.Core;
using System;
using System.Windows.Input;

namespace Mobile.Core.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly IMemberHandler _memberHandler;

        public string UserName { get; set; }
        public string Password { get; set; }

        public LoginViewModel(IMemberHandler memberHandler)
        {
            _memberHandler = memberHandler;
        }
        public override void OnAppear(params object[] args)
        {
            UserName = string.Empty;
            Password = string.Empty;
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
                if (res != null)
                {
                    if (res.Success)
                    {
                        var user = await _memberHandler.TouchLogin();
                        if (user != null)
                        {
                            AppService.CurrentUser = user;
                        }
                    }
                }
                IsBusy = false;
            }
        }
    }
}
