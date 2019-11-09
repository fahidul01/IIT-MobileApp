using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreEngine.Model.DBModel;

namespace Web.Areas.Admin.ViewModels
{
    public class DashboardViewModel
    {
        public List<Lesson> Lessons;
        public List<Notice> Notices;
        public List<User> Batches;

        public DashboardViewModel(List<Lesson> lessons, List<Notice> notices, List<User> batch)
        {
            this.Lessons = lessons;
            this.Notices = notices;
            this.Batches = batch;
        }
    }
}
