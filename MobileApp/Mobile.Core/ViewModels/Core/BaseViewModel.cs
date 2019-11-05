using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Mobile.Core.Engines.Dependency;
using Mobile.Core.Engines.Services;
using System.Windows.Input;

namespace Mobile.Core.ViewModels
{
    public abstract class BaseViewModel : ViewModelBase
    {
        public bool IsBusy { get; set; }
        protected readonly INavigationService _nav;
        public BaseViewModel()
        {
            _nav = Locator.GetInstance<INavigationService>();
        }
        public ICommand RefreshCommand => new RelayCommand(RefreshAction);

        public abstract void OnAppear(params object[] args);

        protected virtual void RefreshAction()
        {

        }

        protected void GoBack()
        {
            _nav.GoBack();
        }
    }
}
