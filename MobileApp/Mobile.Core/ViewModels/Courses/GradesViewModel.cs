using CoreEngine.APIHandlers;
using CoreEngine.Model.DBModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mobile.Core.ViewModels
{
    public class GradesViewModel : BaseViewModel
    {
        private readonly ICourseHandler _courseHandler;
        public List<SemesterData> SemesterDatas { get; set; }
        public GradesViewModel(ICourseHandler courseHandler)
        {
            _courseHandler = courseHandler;
        }
        public override void OnAppear(params object[] args)
        {
            base.OnAppear(args);
            RefreshAction();
        }

        protected override async void RefreshAction()
        {
            base.RefreshAction();
            IsBusy = true;
            var allSemesters = new List<SemesterData>();
            var allcourseData = await _courseHandler.GetResult();
            if (allcourseData != null)
            {
                var grouped = allcourseData.GroupBy(x => x.Course.Semester.Id).ToList();
                foreach (var courseData in grouped)
                {
                    var semesterData = courseData.FirstOrDefault().Course.Semester;
                    var data = new SemesterData(semesterData, courseData.ToList());
                    allSemesters.Add(data);
                }
            }
            SemesterDatas = allSemesters.OrderBy(x => x.Semester.StartsOn).ToList();
            IsBusy = false;
        }
    }

    public class SemesterData
    {
        public SemesterData(Semester semester, List<StudentCourse> studentCourses)
        {
            SemesterName = semester.Name;
            Semester = semester;
            SemesterNo = SemesterName.ToLower().Replace("semester", "").Trim();
            CourseDatas = new List<CourseData>();
            decimal totalCredit = 0;
            foreach (var gradeData in studentCourses)
            {
                var cData = new CourseData(gradeData, gradeData.Course);
                totalCredit += gradeData.GradePoint * gradeData.Course.CourseCredit;
                CourseDatas.Add(new CourseData(gradeData, gradeData.Course));
            }
            SemesterGPA = Math.Round(totalCredit / studentCourses.Sum(x => x.Course.CourseCredit),2);
        }

        public string SemesterNo { get; private set; }
        public string SemesterName { get; private set; }
        public decimal SemesterGPA { get; private set; }
        public List<CourseData> CourseDatas { get; set; }
        public Semester Semester { get; set; }

    }

    public class CourseData
    {
        public string Name { get; private set; }
        public string CourseId { get; private set; }
        public string Grade { get; private set; }
        public decimal GradePoint { get; private set; }
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
