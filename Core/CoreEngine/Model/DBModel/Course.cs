﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CoreEngine.Model.DBModel
{
    public class Course
    {
        public int Id { get; set; }
        public decimal CourseCredit { get; set; }
        public string CourseId { get; set; }
        public string CourseName { get; set; }
        public virtual ICollection<DBUser> Students { get; set; }
        public virtual ICollection<Class> Classes { get; set; }
        [Required]
        public virtual Semester Semester { get; set; }

        public Course()
        {
            Students = new HashSet<DBUser>();
            Classes = new HashSet<Class>();
        }
    }

    public class CourseMaterial
    {
        public int Id { get; set; }
        public string Information { get; set; }
        [Required]
        public virtual Course Course { get; set; }
        public ICollection<string> Files { get; set; }
    }
}
