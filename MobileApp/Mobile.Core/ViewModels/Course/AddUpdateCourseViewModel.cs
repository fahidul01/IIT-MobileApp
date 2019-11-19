using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using CoreEngine.APIHandlers;
using CoreEngine.Model.DBModel;
using GalaSoft.MvvmLight.Command;

namespace Mobile.Core.ViewModels
{
    public class AddUpdateCourseViewModel : BaseViewModel
    {
        private readonly ICourseHandler _courseHandler;

        public Course CurrentCourse { get; private set; }
        public List<Lesson> Lessons { get; private set; }

        public AddUpdateCourseViewModel(ICourseHandler courseHandler)
        {
            _courseHandler = courseHandler;
        }
        public async override void OnAppear(params object[] args)
        {
            base.OnAppear(args);
            if (args != null && args[0] is Course course)
            {
                CurrentCourse = course;
                await LoadLessonsAsync(course);
            }
            else
            {
                CurrentCourse = new Course();
            }
        }

        private async Task LoadLessonsAsync(Course course)
        {
            Lessons = await _courseHandler.GetLessons(course.CourseId);
        }

        public ICommand SaveCommand => new RelayCommand(SaveAction);

        private async void SaveAction()
        {
            if (string.IsNullOrEmpty(CurrentCourse.CourseName)) _dialog.ShowToastMessage("Invalid Name");
            else if (string.IsNullOrEmpty(CurrentCourse.CourseId)) _dialog.ShowToastMessage("Incalid Course ID");
            else if (CurrentCourse.CourseCredit <= 0) _dialog.ShowToastMessage("Credit hour should be greater than 0");
            else
            {
                if (CurrentCourse.Id == 0)
                {
                    var res = await _courseHandler.CreateCourse(CurrentCourse);
                    if (res.Actionstatus) _nav.GoBack();
                    _dialog.ShowToastMessage("Created Course Successfully");
                }
                else
                {
                    var res = await _courseHandler.UpdateCourse(CurrentCourse);
                    if (res.Actionstatus) _nav.GoBack();
                    _dialog.ShowToastMessage("Updated Course Successfully");
                }
            }
        }
    }
}
