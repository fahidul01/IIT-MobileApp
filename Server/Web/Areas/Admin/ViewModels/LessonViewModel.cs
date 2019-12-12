using CoreEngine.Model.DBModel;
using System;
using System.ComponentModel.DataAnnotations;

namespace Web.Areas.Admin.ViewModels
{
    public class LessonViewModel
    {
        public int CourseId { get; set; }
        public int LessonId { get; set; }
        [Required]
        public string TeacherName { get; set; }
        [Required]
        public TimeSpan TimeSpan { get; set; }
        public string RoomNo { get; set; }
        public string Description { get; set; }
        public DayOfWeek DayOfWeek { get; internal set; }

        public LessonViewModel()
        {

        }

        public LessonViewModel(int courseId)
        {
            CourseId = courseId;
        }

        public LessonViewModel(Lesson lesson)
        {
            LessonId = lesson.Id;
            TeacherName = lesson.TeacherName;
            RoomNo = lesson.RoomNo;
            Description = lesson.Description;
        }
    }
}
