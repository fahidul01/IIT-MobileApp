using Mobile.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mobile.Core.Engines.Services
{
    public interface INavigationService
    {
        void GoBack();
        void GoToRoot();
        Task NavigateTo<T>(params object[] parameter) where T : BaseViewModel;
        Task NavigateToModal<T>(params object[] parameter) where T : BaseViewModel;
        void MoveToPage<T>(params object[] parameter) where T : BaseViewModel;
        void GoModalBack();
        void Configure<baseViewModel, page>() where baseViewModel : BaseViewModel;
        void Init<T>() where T : BaseViewModel;
    }
}
