using CoreEngine.APIHandlers;
using CoreEngine.Model.DBModel;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace Mobile.Core.ViewModels
{
    public class CoursesViewModel : BaseViewModel
    {
        private readonly ICourseHandler _courseHandler;

        public List<Semester> Semesters { get; private set; }
        public CoursesViewModel(ICourseHandler courseHandler)
        {
            _courseHandler = courseHandler;
        }

        public override void OnAppear(params object[] args)
        {
            base.OnAppear(args);
            RefreshAction();
        }

        protected async override void RefreshAction()
        {
            base.RefreshAction();
            Semesters = await _courseHandler.GetCurrentSemester();
        }

        public ICommand CourseCommand => new RelayCommand<Course>(CourseAction);

        private void CourseAction(Course obj)
        {
            _nav.NavigateTo<CourseDetailViewModel>(obj);
        }
    }
}
