using System.ComponentModel.DataAnnotations;

namespace CoreEngine.Model.DBModel
{
    public class Grade
    {
        public int Id { get; set; }
        public string GradeName { get; set; }
        public decimal GradePoint { get; set; }
        [Required]
        public virtual StudentCourse StudentCourse { get; set; }
    }
}
