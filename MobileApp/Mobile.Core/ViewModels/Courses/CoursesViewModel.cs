using CoreEngine.Model.DBModel;
using System.Collections.Generic;

namespace Mobile.Core.ViewModels
{
    public class CoursesViewModel : BaseViewModel
    {
        public List<Course> Courses { get; private set; }
    }
}
