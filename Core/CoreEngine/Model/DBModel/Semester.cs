using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

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

    public class SemesterData
    {
        public SemesterData(Semester semester, List<StudentCourse> studentCourses)
        {
            SemesterName = semester.Name;
            StartsOn = semester.StartsOn;
            SemesterNo = SemesterName.ToLower().Replace("semester", "").Trim();
            CourseDatas = new List<CourseData>();
            decimal totalCredit = 0;
            foreach (var gradeData in studentCourses)
            {
                var cData = new CourseData(gradeData, gradeData.Course);
                totalCredit += gradeData.GradePoint * gradeData.Course.CourseCredit;
                CourseDatas.Add(new CourseData(gradeData, gradeData.Course));
            }
            SemesterGPA = Math.Round(totalCredit / studentCourses.Sum(x => x.Course.CourseCredit), 2);
        }

        public string SemesterNo { get; set; }
        public string SemesterName { get; set; }
        public DateTime StartsOn { get; set; }
        public decimal SemesterGPA { get; set; }
        public List<CourseData> CourseDatas { get; set; }
    }

    public class CourseData
    {
        public string Name { get; set; }
        public string CourseId { get; set; }
        public string Grade { get; set; }
        public decimal GradePoint { get; set; }
        public bool Failed => Grade == "F" || Grade == "f";

        public CourseData(StudentCourse studentCourse, Course course)
        {
            Name = course.CourseName;
            CourseId = course.CourseId;
            GradePoint = studentCourse.GradePoint;
            if (string.IsNullOrEmpty(studentCourse.Grade))
            {
                Grade = "Not Published";
            }
            else
            {
                Grade = studentCourse.Grade;
            }
        }
    }
}
