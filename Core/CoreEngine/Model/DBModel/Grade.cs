using System.ComponentModel.DataAnnotations;

namespace CoreEngine.Model.DBModel
{
    public class Grade
    {
        public int Id { get; set; }
        public string GradeName { get; set; }
        public decimal GradePoint { get; set; }
        [Required]
        public virtual Course Course { get; set; }
        [Required]
        public virtual DBUser Student { get; set; }
    }
}
