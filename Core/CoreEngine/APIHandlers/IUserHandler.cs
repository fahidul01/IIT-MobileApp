using CoreEngine.Model.DBModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreEngine.APIHandlers
{
    public interface IUserHandler
    {
        Task<bool> CreateUser(User user);
        Task<List<User>> GetUsers();
        Task<bool> UpdateUser(User user);
        Task<bool> DeleteUser(User user);
    }
}
