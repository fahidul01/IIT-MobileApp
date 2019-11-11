using Mobile.Core.Engines.Services;
using Mobile.Core.ViewModels;
using MobileApp.Views.Login;
using MobileApp.Views.Splash;

namespace MobileApp.Service
{
    internal class ViewService
    {
        internal void Init(INavigationService nav)
        {
            nav.Configure<SplashViewModel, SplashPage>();
            nav.Configure<LoginViewModel, LoginPage>();
        }
    }
}
