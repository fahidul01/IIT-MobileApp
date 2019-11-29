using CoreEngine.APIHandlers;
using CoreEngine.Model.DBModel;
using Mobile.Core.Models.Core;

namespace Mobile.Core.ViewModels
{
    public class CourseDetailViewModel : BaseViewModel
    {
        private readonly ICourseHandler _courseHandler;
        public bool CanEdit { get; private set; }

        public Course CurrentCourse { get; private set; }

        public CourseDetailViewModel(ICourseHandler courseHandler)
        {
            _courseHandler = courseHandler;
        }
        public override void OnAppear(params object[] args)
        {
            base.OnAppear(args);
            if (args != null && args.Length > 0 && args[0] is Course course)
            {
                CurrentCourse = course;
                CanEdit = AppService.HasCRRole;
                RefreshAction();
            }
            else
            {
                GoBack();
            }
        }

        protected async override void RefreshAction()
        {
            base.RefreshAction();
            IsBusy = true;
            CurrentCourse = await _courseHandler.GetCourse(CurrentCourse.Id);
            IsBusy = false;
        }
    }
}
