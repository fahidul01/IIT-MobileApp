using CoreEngine.Model.Common;
using CoreEngine.Model.DBModel;
using Microsoft.EntityFrameworkCore;
using Student.Infrastructure.DBModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Student.Infrasructure.Services
{
    public class TodoItemService : BaseService
    {
        private readonly StudentDBContext _db;

        public TodoItemService(StudentDBContext dBContext)
        {
            _db = dBContext;
        }
        public async Task<ActionResponse> Add(string userId, ToDoItem toDoItem)
        {
            if (toDoItem.OwnerId != userId)
            {
                return new ActionResponse(false, "Invalid User Information");
            }
            foreach (var user in toDoItem.ParticementUserIds)
            {
                var dbUser = await _db.DBUsers.FindAsync(user);
                if (dbUser != null)
                {
                    var mergedItem = new DBUserTodoItem()
                    {
                        DBUser = dbUser
                    };
                    toDoItem.Participents.Add(mergedItem);
                }
            }
            _db.Entry(toDoItem).State = EntityState.Added;
            await _db.SaveChangesAsync();
            return new ActionResponse(true);
        }

        public async Task<ActionResponse> Delete(string userId, ToDoItem toDoItem)
        {
            if (toDoItem.OwnerId != userId)
            {
                return new ActionResponse(false, "Invalid User Information");
            }
            _db.Entry(toDoItem).State = EntityState.Deleted;
            await _db.SaveChangesAsync();
            return new ActionResponse(true);
        }

        public async Task<List<ToDoItem>> GetItem(string userId, int page)
        {
            var items = await _db.ToDoItems.Include(x => x.Participents)
                                           .Where(x => x.Participents
                                                        .Any(m => m.DBUser.Id == userId))
                                           .Skip(page * 20)
                                           .Take(20)
                                           .ToListAsync();
            return items;
        }

        public async Task<ActionResponse> Update(string userId, ToDoItem toDoItem)
        {
            if (toDoItem.OwnerId != userId)
            {
                return new ActionResponse(false, "Invalid User Information");
            }
            toDoItem.Participents.Clear();
            foreach (var user in toDoItem.ParticementUserIds)
            {
                var dbUser = await _db.DBUsers.FindAsync(user);
                if (dbUser != null)
                {
                    var mergedItem = new DBUserTodoItem()
                    {
                        DBUser = dbUser
                    };
                    toDoItem.Participents.Add(mergedItem);
                }
            }
            _db.Entry(toDoItem).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            return new ActionResponse(true);
        }
    }
}
