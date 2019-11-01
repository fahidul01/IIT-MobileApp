using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CoreEngine.Model.DBModel
{
    public class Semester
    {
        public int Id { get; set; }
        [Required]
        public virtual Batch Batch { get; set; }
        public string Name { get; set; }
        /// <summary>
        /// Duration of a semester in Months
        /// </summary>
        public int Duration { get; set; }
        public DateTime StartsOn { get; set; }
        public DateTime EndsOn { get; set; }
        public virtual ICollection<Course> Courses { get; set; }

        public Semester()
        {
            Courses = new HashSet<Course>();
        }
    }
}
