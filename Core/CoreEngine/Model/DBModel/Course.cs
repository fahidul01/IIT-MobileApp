using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CoreEngine.Model.DBModel
{
    public class Course : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public decimal CourseCredit { get; set; }
        public string CourseId { get; set; }
        public string CourseName { get; set; }
        public string Description { get; set; }
        public virtual ICollection<StudentCourse> StudentCourses { get; set; }
        public virtual ICollection<Lesson> Lessons { get; set; }
        public virtual ICollection<DBFile> CourseMaterials { get; set; }
        [Required]
        public virtual Semester Semester { get; set; }

        public Course()
        {
            StudentCourses = new HashSet<StudentCourse>();
            Lessons = new HashSet<Lesson>();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyChange(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }

    public class StudentCourse
    {
        public int Id { get; set; }
        [Required]
        public virtual Course Course { get; set; }
        [Required]
        public virtual DBUser Student { get; set; }
        public string Grade { get; set; }
        public decimal GradePoint { get; set; }
    }
}
