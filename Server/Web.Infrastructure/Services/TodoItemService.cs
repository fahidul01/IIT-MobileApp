using CoreEngine.Model.Common;
using CoreEngine.Model.DBModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Student.Infrasructure.Services
{
    public class TodoItemService : BaseService
    {
        public Task<ActionResponse> Add(string userId, ToDoItem toDoItem)
        {
            throw new NotImplementedException();
        }

        public Task<ActionResponse> Delete(string userId, ToDoItem toDoItem)
        {
            throw new NotImplementedException();
        }

        public Task<List<ToDoItem>> GetItem(string userId, int page)
        {
            throw new NotImplementedException();
        }

        public Task<ActionResponse> Update(string userId, ToDoItem toDoItem)
        {
            throw new NotImplementedException();
        }
    }
}
