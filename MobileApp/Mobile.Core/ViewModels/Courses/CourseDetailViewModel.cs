using CoreEngine.APIHandlers;
using CoreEngine.Model.DBModel;
using GalaSoft.MvvmLight.Command;
using Mobile.Core.Models.Core;
using System;
using System.Collections.Generic;
using System.Windows.Input;

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

        protected override async void RefreshAction()
        {
            base.RefreshAction();
            IsBusy = true;
            CurrentCourse = await _courseHandler.GetCourse(CurrentCourse.Id);

            IsBusy = false;
        }

        public ICommand EditCourseCommand => new RelayCommand(CourseAction);

        private void CourseAction()
        {
            var actionList = new Dictionary<string, Action>();

            actionList.Add("Add Event", () => _nav.NavigateTo<AddUpdateNoticeViewModel>(PostType.ClassCancel, CurrentCourse));
            actionList.Add("Modify Course", () => _nav.NavigateTo<AddUpdateCourseViewModel>(CurrentCourse));
            actionList.Add("Add Lesson", () => _nav.NavigateTo<AddUpdateLessonViewModel>(CurrentCourse));
            actionList.Add("Add Course Material", AddMaterialAction);
            actionList.Add("Upload Course Grade", CourseGradeAction);

            _dialog.ShowAction(CurrentCourse.CourseName, "Cancel", actionList);
        }

        private void CourseGradeAction()
        {

        }

        private void AddMaterialAction()
        {

        }
    }
}
