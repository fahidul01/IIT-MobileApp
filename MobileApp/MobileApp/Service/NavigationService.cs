using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mobile.Core.Engines.Services;
using Mobile.Core.ViewModels;
using Xamarin.Forms;

namespace MobileApp.Service
{
    public class NavigationService : INavigationService , IDialogService
    {
        private bool OnNavigating;
        private readonly Dictionary<Type, Type> Pages;
        private readonly IPlatformService _platformService;
        private NavigationPage _nav;
        public NavigationService()
        {
            Pages = new Dictionary<Type, Type>();
            _platformService = AppService.PlatformService;
        }

        public void Init<T>() where T: BaseViewModel
        {
            OnNavigating = true;
            var vm = typeof(T);

            var page = Activator.CreateInstance(Pages[vm]) as Page;
            _nav = new NavigationPage(page)
            {
                BarBackgroundColor = Color.Transparent,
                BarTextColor = Color.White
            };
            Application.Current.MainPage = _navigation;

            page.BindingContext = Locator.GetInstance<T>();

            if (page.BindingContext is BaseViewModel viewModel)
            {
                viewModel.OnAppearing();
            }

            OnNavigating = false;
            _navigation.Popped += (s, e) =>
            {
                var currentPage = _navigation.CurrentPage as CustomPage;
                _navigation.BarBackgroundColor = currentPage.Transparent ? Color.Transparent : PrimaryColor;
            };
        }

        public void GoBack()
        {
            throw new NotImplementedException();
        }

        public void GoModalBack()
        {
            throw new NotImplementedException();
        }

        public void GoToRoot()
        {
            throw new NotImplementedException();
        }

       

        public void MoveToPage<T>(params object[] parameter) where T : BaseViewModel
        {
            throw new NotImplementedException();
        }

        public Task NavigateTo<T>(params object[] parameter) where T : BaseViewModel
        {
            throw new NotImplementedException();
        }

        public Task NavigateToModal<T>(params object[] parameter) where T : BaseViewModel
        {
            throw new NotImplementedException();
        }

        public void Configure<baseViewModel, page>() where baseViewModel : BaseViewModel
        {

        }

        public void ShowMessage(string title, string message)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ShowConfirmation(string title, string meaage)
        {
            throw new NotImplementedException();
        }

        public Task<string> ShowAction(string title, string cancel, params object[] actions)
        {
            throw new NotImplementedException();
        }

        public void ShowAction(string title, string cancel, Dictionary<string, Action> actions)
        {
            throw new NotImplementedException();
        }

        public void ShowToastMessage(string message)
        {
            throw new NotImplementedException();
        }
    }
}
