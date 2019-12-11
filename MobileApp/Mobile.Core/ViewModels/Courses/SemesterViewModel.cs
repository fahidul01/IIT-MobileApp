using CoreEngine.Model.DBModel;
using GalaSoft.MvvmLight.Command;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace Mobile.Core.ViewModels
{
    public class SemesterViewModel : BaseViewModel
    {
        public List<Course> Courses { get; set; }
        public Semester CurrentSemester { get; private set; }
        public SemesterViewModel()
        {

        }

        public override void OnAppear(params object[] args)
        {
            base.OnAppear(args);
            if (args.Length > 0 && args[0] is Semester semester)
            {
                Courses = semester.Courses.ToList();
                CurrentSemester = semester;
            }
        }

        public ICommand CourseCommand => new RelayCommand<Course>(CourseAction);
        public ICommand AddCommand => new RelayCommand(AddAction);

        private void AddAction()
        {
            _nav.NavigateTo<AddUpdateCourseViewModel>(CurrentSemester);
        }

        private void CourseAction(Course obj)
        {
            obj.Semester = CurrentSemester;
            _nav.NavigateTo<CourseDetailViewModel>(obj);
        }
    }
}
