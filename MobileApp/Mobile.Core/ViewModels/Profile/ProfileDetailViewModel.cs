using CoreEngine.Model.DBModel;
using System.Collections.Generic;

namespace Mobile.Core.ViewModels
{
    public class ProfileDetailViewModel : BaseViewModel
    {
        private User _currentUser;
        public List<Semester> Semesters { get; set; }
        public override void OnAppear(params object[] args)
        {
            if (args != null && args.Length > 0 && args[0] is User user)
            {
                _currentUser = user;
            }
            else
            {
                GoBack();
            }
        }
    }
}
