using CoreEngine.Model.DBModel;
using System.Collections.Generic;

namespace Web.Areas.Admin.ViewModels
{
    public class DashboardViewModel
    {
        public List<Lesson> Lessons;
        public List<Notice> Notices;
        public List<User> Batches;

        public DashboardViewModel(List<Lesson> lessons, List<Notice> notices, List<User> batch)
        {
            Lessons = lessons;
            Notices = notices;
            Batches = batch;
        }
    }
}
