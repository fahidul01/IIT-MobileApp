using CoreEngine.APIHandlers;
using CoreEngine.Model.DBModel;
using System.Collections.Generic;

namespace Mobile.Core.ViewModels
{
    public class ProfileDetailViewModel : BaseViewModel
    {
        private readonly IMemberHandler _memberHandler;
        private readonly ICourseHandler _courseHandler;

        public User CurrentUser { get; private set; }
        public List<Semester> Semesters { get; set; }
        public ProfileDetailViewModel(IMemberHandler memberHandler, ICourseHandler courseHandler)
        {
            _memberHandler = memberHandler;
            _courseHandler = courseHandler;
        }
        public override void OnAppear(params object[] args)
        {
            if (args != null && args.Length > 0 && args[0] is User user)
            {
                CurrentUser = user;
                LoadUserDetail();
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
            Semesters = await _courseHandler.GetCurrentSemester();
        }
    }
}
