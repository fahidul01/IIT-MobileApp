using System.ComponentModel.DataAnnotations;

namespace Web.Areas.Admin.ViewModels
{
    public class CreateCoursePopupModel
    {
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

        public CreateCoursePopupModel(int semester, int batch)
        {
            SemesterId = semester;
            BatchId = batch;
        }
    }
}
