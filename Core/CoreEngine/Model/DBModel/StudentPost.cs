using System;
using System.Collections.Generic;
using System.Text;

namespace CoreEngine.Model.DBModel
{
    public class StudentPost
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public PostType PostType { get; set; }
        public bool FutureNotification { get; set; }
        public DateTime EventDate { get; set; }
        public string File { get; set; }

    }

    public enum PostType
    {
        Notice,
        Examination,
        ClassCancel,
        ExtraClass
    }
}
