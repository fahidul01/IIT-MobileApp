using System;
using System.ComponentModel.DataAnnotations;

namespace CoreEngine.Model.DBModel
{
    public class Class
    {
        public int Id { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
        public TimeSpan TimeOfDay { get; set; }
        [Required]
        public virtual Batch Batch { get; set; }
        [Required]
        public virtual Course Course { get; set; }
    }
}
