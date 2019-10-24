using System;
using System.Collections.Generic;
using System.Text;

namespace CoreEngine.Model.DBModel
{
    public class Batch
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<DBUser> Students { get; set; }
        public virtual ICollection<Semester> Semesters { get; set; }
        public DateTime StartsOn { get; set; }

        public Batch()
        {
            Students = new HashSet<DBUser>();
            Semesters = new HashSet<Semester>();
        }
    }
}
