using CoreEngine.Model.DBModel;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace Mobile.Core.ViewModels
{
    public class AddUpdateLessonViewModel:BaseViewModel
    {
        public Course CurrentCourse { get; private set; }
        public Lesson Lesson { get; private set; }

        public override void OnAppear(params object[] args)
        {
            base.OnAppear(args);
            if (args != null && args.Length == 2 && args[0] is Course course && args[1] is Lesson lesson)
            {
                CurrentCourse = course;
                Lesson = lesson;
            }
            else GoBack();
        }

        public ICommand SaveCommand => new RelayCommand(SaveAction);

        private void SaveAction()
        {
            throw new NotImplementedException();
        }
    }
}
