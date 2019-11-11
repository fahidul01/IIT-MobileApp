using System;

namespace Mobile.Core.ViewModels
{
    public class SplashViewModel : BaseViewModel
    {
        public override void OnAppear(params object[] args)
        {
            _nav.NavigateTo<LoginViewModel>();
        }
    }
}
