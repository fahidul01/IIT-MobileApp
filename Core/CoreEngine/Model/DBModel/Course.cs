using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CoreEngine.Model.DBModel
{
    public class Course
    {
        public int Id { get; set; }
        public decimal CourseCredit { get; set; }
        public string CourseId { get; set; }
        public string CourseName { get; set; }
        public virtual ICollection<StudentCourse> StudentCourses { get; set; }
        public virtual ICollection<Class> Classes { get; set; }
        public virtual ICollection<CourseMaterial> CourseMaterials { get; set; }
        [Required]
        public virtual Semester Semester { get; set; }

        public Course()
        {
            StudentCourses = new HashSet<StudentCourse>();
            Classes = new HashSet<Class>();
        }
    }

    public class CourseMaterial
    {
        public int Id { get; set; }
        public string Information { get; set; }
        [Required]
        public virtual Course Course { get; set; }
        public ICollection<DBFile> Files { get; set; }
    }

    public class StudentCourse
    {
        public int Id { get; set; }
        [Required]
        public virtual Course Course { get; set; }
        [Required]
        public virtual DBUser Student { get; set; }

        //ToDO
        public int GradeId { get; set; }
        public virtual Grade Grade { get; set; }
    }
}
