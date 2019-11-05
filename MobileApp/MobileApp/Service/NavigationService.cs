﻿using Mobile.Core.Engines.Dependency;
using Mobile.Core.Engines.Services;
using Mobile.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MobileApp.Service
{
    public class NavigationService : INavigationService, IDialogService
    {
        private readonly SemaphoreSlim _semaphoreSlim;
        private readonly Dictionary<Type, Type> Pages;
        private readonly IPlatformService _platformService;
        private NavigationPage _nav;
        public NavigationService(IPlatformService platformService)
        {
            Pages = new Dictionary<Type, Type>();
            _platformService = platformService;
            _semaphoreSlim = new SemaphoreSlim(1, 1);
        }

        public async void Init<T>() where T : BaseViewModel
        {
            await _semaphoreSlim.WaitAsync();
            var vm = typeof(T);

            var page = Activator.CreateInstance(Pages[vm]) as Page;
            _nav = new NavigationPage(page)
            {
                BarBackgroundColor = Color.Transparent,
                BarTextColor = Color.White
            };
            Application.Current.MainPage = _nav;

            page.BindingContext = Locator.GetInstance<T>();

            if (page.BindingContext is BaseViewModel viewModel)
            {
                viewModel.OnAppear();
            }

            _semaphoreSlim.Release();
            //_nav.Popped += (s, e) =>
            //{
            //    var currentPage = _nav.CurrentPage as CustomPage;
            //    _navigation.BarBackgroundColor = currentPage.Transparent ? Color.Transparent : PrimaryColor;
            //};
        }

        public async void GoBack()
        {
            await _semaphoreSlim.WaitAsync();
            await _nav.Navigation.PopAsync();
            _semaphoreSlim.Release();
        }

        public async void GoModalBack()
        {
            await _semaphoreSlim.WaitAsync();
            await _nav.Navigation.PopModalAsync();
            _semaphoreSlim.Release();
        }

        public async void GoToRoot()
        {
            await _semaphoreSlim.WaitAsync();
            await _nav.Navigation.PopToRootAsync();
            _semaphoreSlim.Release();
        }


        public async Task NavigateTo<T>(params object[] parameter) where T : BaseViewModel
        {
            await _semaphoreSlim.WaitAsync();
            var type = typeof(T);
            if (Pages.ContainsKey(type))
            {
                var page = Activator.CreateInstance(Pages[type]) as Page;
                var vm = Locator.GetInstance<T>();
                page.BindingContext = vm;
                if (page.BindingContext is BaseViewModel viewModel)
                {
                    viewModel.OnAppear(parameter);
                }
                await _nav.PushAsync(page);
            }
            else
            {
                ShowMessage("Error", "Invalid Page");
            }
            _semaphoreSlim.Release();
        }

        public async Task NavigateToModal<T>(params object[] parameter) where T : BaseViewModel
        {
            await _semaphoreSlim.WaitAsync();
            var type = typeof(T);
            if (Pages.ContainsKey(type))
            {
                var page = Activator.CreateInstance(Pages[type]) as Page;
                var vm = Locator.GetInstance<T>();
                page.BindingContext = vm;
                if (page.BindingContext is BaseViewModel viewModel)
                {
                    viewModel.OnAppear(parameter);
                }
                await _nav.Navigation.PushModalAsync(page);
            }
            else
            {
                ShowMessage("Error", "Invalid Page");
            }
            _semaphoreSlim.Release();
        }

        public void Configure<baseViewModel, page>() where baseViewModel : BaseViewModel
        {
            Pages.Add(typeof(baseViewModel), typeof(page));
        }

        public void ShowMessage(string title, string message)
        {
            _nav.DisplayAlert(title, message, "Ok");
        }

        public async Task<bool> ShowConfirmation(string title, string message)
        {
            var res = await _nav.DisplayPromptAsync(title, message, "Ok");
            return res == "Ok";
        }

        public async Task<string> ShowAction(string title, string cancel, params string[] actions)
        {
            var res = await _nav.DisplayActionSheet(title, cancel, "", actions);
            return res;
        }

        public async void ShowAction(string title, string cancel, Dictionary<string, Action> actions)
        {
            var buttons = actions.Keys.ToArray();
            var res = await ShowAction(title, cancel, buttons);
            if (!string.IsNullOrWhiteSpace(res) && actions.ContainsKey(res))
            {
                actions[res].Invoke();
            }
        }

        public void ShowToastMessage(string message)
        {
            _platformService.OpenToast(message);
        }
    }
}
