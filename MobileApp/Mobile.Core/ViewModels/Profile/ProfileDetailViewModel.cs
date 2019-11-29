using CoreEngine.APIHandlers;
using CoreEngine.Model.DBModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mobile.Core.ViewModels
{
    public class ProfileDetailViewModel : BaseViewModel
    {
        private readonly IMemberHandler _memberHandler;

        public User CurrentUser { get; private set; }
        public List<Semester> Semesters { get; set; }
        public ProfileDetailViewModel(IMemberHandler memberHandler)
        {
            _memberHandler = memberHandler;
        }
        public override void OnAppear(params object[] args)
        {
            if (args != null && args.Length > 0 && args[0] is User user)
            {
                CurrentUser = user;
                LoadUserDetail(user.Id);
            }
            else
            {
                GoBack();
            }
        }

        protected override void RefreshAction()
        {
            base.RefreshAction();
            LoadUserDetail();
        }

        private async void LoadUserDetail()
        {
            Semesters = await _memberHandler.GetCurrentSemester();
        }
    }
}
