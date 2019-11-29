using CoreEngine.APIHandlers;
using CoreEngine.Model.DBModel;
using GalaSoft.MvvmLight.Command;
using System.Collections.Generic;
using System.Windows.Input;

namespace Mobile.Core.ViewModels
{
    public class ProfilesViewModel : BaseViewModel
    {
        private readonly IMemberHandler _memberHandler;
        public List<User> CurrentStudents { get; set; }

        public ProfilesViewModel(IMemberHandler userHandler)
        {
            _memberHandler = userHandler;
        }

        public override void OnAppear(params object[] args)
        {
            LoadCurrentStudents();
        }

        protected override void RefreshAction()
        {
            base.RefreshAction();
            LoadCurrentStudents();
        }

        private async void LoadCurrentStudents()
        {
            IsBusy = true;
            CurrentStudents = await _memberHandler.GetCurrentBatchUsers();
            IsBusy = false;
        }

        public ICommand ViewProfileCommand => new RelayCommand<User>(ViewProfileAction);

        private void ViewProfileAction(User obj)
        {
            _nav.NavigateTo<ProfileDetailViewModel>(obj);
        }
    }
}
