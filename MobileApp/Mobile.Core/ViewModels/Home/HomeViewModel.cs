using CoreEngine.APIHandlers;
using CoreEngine.Model.DBModel;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace Mobile.Core.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        private readonly ILessonHandler _classHandler;
        private readonly ICourseHandler _courseHandler;
        private readonly INoticeHandler _noticeHandler;
        public List<Notice> UpcomingEvents { get; set; }
        public List<Notice> RecentNotices { get; set; }
        public List<Lesson> UpcomingClasses { get; set; }
        public List<Course> CurrentCourses { get; set; }

        public HomeViewModel(ILessonHandler classHandler, ICourseHandler courseHandler, INoticeHandler postHandler)
        {
            _classHandler = classHandler;
            _courseHandler = courseHandler;
            _noticeHandler = postHandler;
        }
        public override void OnAppear(params object[] args)
        {
            LoadEvents();
        }

        private async void LoadEvents()
        {
            IsBusy = true;
            UpcomingClasses = await _classHandler.GetLessons();
            UpcomingEvents = await _noticeHandler.GetUpcomingEvents(1, PostType.All);
            RecentNotices = await _noticeHandler.GetPosts(1, PostType.All);
            CurrentCourses = await _courseHandler.GetCourses();
        }

        public ICommand SelectLessonCommand => new RelayCommand<Lesson>(SelectLessonAction);
        public ICommand SelectNoticeCommand => new RelayCommand<Notice>(SelectNoticeAction);
        public ICommand SelectCourseCommand => new RelayCommand<Course>(SelectCourseAction);

        private void SelectCourseAction(Course obj)
        {
            throw new NotImplementedException();
        }

        private void SelectNoticeAction(Notice obj)
        {
            throw new NotImplementedException();
        }

        private void SelectLessonAction(Lesson obj)
        {
            throw new NotImplementedException();
        }
    }
}
