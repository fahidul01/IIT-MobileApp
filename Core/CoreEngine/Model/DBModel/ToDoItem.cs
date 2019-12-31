using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoreEngine.Model.DBModel
{
    public class ToDoItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public DateTime EventTime { get; set; }
        public string OwnerId { get; set; }
        public List<DBUserTodoItem> Participents { get; set; }
        [NotMapped]
        public List<string> ParticementUserIds { get; set; }
    }

    public class DBUserTodoItem
    {
        public int Id { get; set; }
        [Required]
        public virtual DBUser DBUser { get; set; }
        [Required]
        public virtual ToDoItem ToDoItem { get; set; }
     }
}
