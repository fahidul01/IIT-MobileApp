using System;
using System.Collections.Generic;

namespace CoreEngine.Model.DBModel
{
    public class ToDoItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public DateTime EventTime { get; set; }
        public DBUser Owner { get; set; }
        public List<DBUser> Participents { get; set; }
    }
}
