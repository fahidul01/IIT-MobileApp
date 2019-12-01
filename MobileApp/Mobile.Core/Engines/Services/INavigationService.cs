using Mobile.Core.ViewModels;
using System;
using System.Threading.Tasks;

namespace Mobile.Core.Engines.Services
{
    public interface INavigationService
    {
        void GoBack();
        void GoToRoot();
        Task NavigateTo<T>(params object[] parameter) where T : BaseViewModel;
        Task NavigateTo(Type T, params object[] parameter);
        Task NavigateToModal<T>(params object[] parameter) where T : BaseViewModel;
        void GoModalBack();
        void Configure(Type viewModel, Type page);
        void Init<T>() where T : BaseViewModel;
    }
}
