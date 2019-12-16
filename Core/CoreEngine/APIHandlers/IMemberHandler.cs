using CoreEngine.Model.Common;
using CoreEngine.Model.DBModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreEngine.APIHandlers
{
    public interface IMemberHandler
    {
        Task<SignInResponse> Login(string username, string password);
        Task<ActionResponse> ChangePassword(string currentPassword, string newPassword);
        Task<ActionResponse> Register(User user);
        Task<ActionResponse> ForgetPassword(string username);
        Task<User> TouchLogin();
        void Logout();

        Task<List<User>> GetCurrentBatchUsers();
        Task<ActionResponse> UpdateUser(User user);
        Task<ActionResponse> DeleteUser(User user);
        Task<List<User>> SearchStudentsAsync(string key);
    }
}
