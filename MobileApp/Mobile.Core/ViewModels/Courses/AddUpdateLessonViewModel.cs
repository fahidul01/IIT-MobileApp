using CoreEngine.APIHandlers;
using CoreEngine.Model.DBModel;
using GalaSoft.MvvmLight.Command;
using System.Windows.Input;

namespace Mobile.Core.ViewModels
{
    public class AddUpdateLessonViewModel : BaseViewModel
    {
        private readonly ILessonHandler _lessonHandler;

        public Course CurrentCourse { get; private set; }
        public Lesson Lesson { get; private set; }

        public AddUpdateLessonViewModel(ILessonHandler lessonHandler)
        {
            _lessonHandler = lessonHandler;
        }

        public override void OnAppear(params object[] args)
        {
            base.OnAppear(args);
            if (args != null && args.Length == 2 && args[0] is Course course && args[1] is Lesson lesson)
            {
                CurrentCourse = course;
                Lesson = lesson;
            }
            else
            {
                GoBack();
            }
        }

        public ICommand SaveCommand => new RelayCommand(SaveActionAsync);

        private async void SaveActionAsync()
        {
            if (Lesson.Id == 0)
            {
                var res = await _lessonHandler.AddLesson(CurrentCourse.Id, Lesson);
            }
        }
    }
}
