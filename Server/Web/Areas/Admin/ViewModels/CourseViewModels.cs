using CoreEngine.Model.DBModel;
using System.ComponentModel.DataAnnotations;

namespace Web.Areas.Admin.ViewModels
{
    public class CreateCoursePopupModel
    {
        public Semester Semester { get; private set; }
        [Required]
        public int SemesterId { get; set; }
        [Required]
        public int BatchId { get; set; }
        [Required]
        [Display(Name = "Course Name")]
        public string CourseName { get; set; }
        [Required]
        [Display(Name = "Course Id")]
        public string CourseId { get; set; }
        [Required]
        [Range(0.5,10)]
        [Display(Name = "Total Course Credit")]
        public decimal CourseCredit { get; set; }
        public CreateCoursePopupModel()
        {

        }

        public CreateCoursePopupModel(Semester semester, int batch)
        {
            Semester = semester;
            SemesterId = semester.Id;
            BatchId = batch;
        }
    }
}
