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
        public string CourseName { get; set; }
        [Required]
        public string CourseId { get; set; }
        [Required]
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
