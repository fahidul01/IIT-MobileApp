using CoreEngine.Model.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CoreEngine.Model.DBModel
{
    public class Notice: BaseModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public PostType PostType { get; set; }
        public bool FutureNotification { get; set; } = true;
        public DateTime CreatedOn { get; set; }
        public DateTime EventDate { get; set; }
        public ICollection<DBFile> DBFiles { get; set; }
        public virtual Batch Batch { get; set; }
        [Required]
        public virtual DBUser Owner { get; set; }
        public Notice()
        {
            DBFiles = new HashSet<DBFile>();
            CreatedOn = CurrentTime;
            EventDate = CurrentTime;
        }
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
