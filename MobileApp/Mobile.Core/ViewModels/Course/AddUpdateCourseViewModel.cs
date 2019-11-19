using System;
using CoreEngine.APIHandlers;
using CoreEngine.Model.DBModel;

namespace Mobile.Core.ViewModels
{
    public class AddUpdateCourseViewModel : BaseViewModel
    {
        private readonly ICourseHandler _courseHandler;

        public Course CurrentCourse { get; private set; }
        public AddUpdateCourseViewModel(ICourseHandler courseHandler)
        {
            _courseHandler = courseHandler;
        }
        public override void OnAppear(params object[] args)
        {
            base.OnAppear(args);
            if (args != null && args[0] is Course course)
            {
                CurrentCourse = course;
                LoadLessonsAsync(course);
            }
        }

        private async System.Threading.Tasks.Task LoadLessonsAsync(Course course)
        {
            Lessons = await _courseHandler.GetLessons(course.CourseId);
        }
    }
}
