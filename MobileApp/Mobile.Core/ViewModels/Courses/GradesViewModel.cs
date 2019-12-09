using CoreEngine.APIHandlers;
using CoreEngine.Model.DBModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Mobile.Core.ViewModels
{
    public class GradesViewModel : BaseViewModel
    {
        private readonly ICourseHandler _courseHandler;
        public ObservableCollection<SemesterData> SemesterDatas { get; private set; }
        public GradesViewModel(ICourseHandler courseHandler)
        {
            _courseHandler = courseHandler;
            SemesterDatas = new ObservableCollection<SemesterData>();
        }
        public override void OnAppear(params object[] args)
        {
            base.OnAppear(args);
            RefreshAction();
        }

        protected async override void RefreshAction()
        {
            base.RefreshAction();
            SemesterDatas.Clear();
            IsBusy = true;
            var allcourseData = await _courseHandler.GetResult();
            if(allcourseData!= null)
            {
                var grouped = allcourseData.GroupBy(x => x.Course.Semester).ToList();
                foreach(var semester in grouped.OrderBy(x => x.Key.StartsOn))
                {
                    var data = new SemesterData(semester.Key, semester.ToList());
                    SemesterDatas.Add(data);
                }
            }
            IsBusy = false;
        }
    }

    public class SemesterData
    {
        public SemesterData(Semester semester, List<StudentCourse> studentCourses)
        {
            SemesterName = semester.Name;

            decimal totalCredit = 0;
            foreach(var gradeData in studentCourses)
            {
                var cData = new CourseData(gradeData, gradeData.Course);
                totalCredit += gradeData.GradePoint * gradeData.Course.CourseCredit;
            }
            SemesterGPA = Math.Round(totalCredit / studentCourses.Sum(x => x.Course.CourseCredit));
        }

        public string SemesterName { get; private set; }
        public decimal SemesterGPA { get; private set; }
        public List<CourseData> CourseDatas { get; private set; }

    }

    public class CourseData
    {
        public string Name { get;private set; }
        public string CourseId { get;private set; }
        public string Grade { get;private set; }
        public decimal GradePoint { get;private set; }
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
            else Grade = studentCourse.Grade;
        }
    }
}
