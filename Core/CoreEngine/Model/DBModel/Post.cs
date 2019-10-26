using System;
using System.ComponentModel.DataAnnotations;

namespace CoreEngine.Model.DBModel
{
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public PostType PostType { get; set; }
        public bool FutureNotification { get; set; }
        public DateTime EventDate { get; set; }
        public string File { get; set; }
        public virtual Batch Batch { get; set; }
        [Required]
        public virtual DBUser Owner { get; set; }
    }

    public enum PostType
    {
        Notice,
        Examination,
        ClassCancel,
        ExtraClass,
        All
    }
}
