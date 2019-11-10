using CoreEngine.Model.Common;
using CoreEngine.Model.DBModel;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace CoreEngine.APIHandlers
{
    public interface IMemberHandler
    {
        Task<SignInResponse> Login(string username, string password);
        Task<SignInResponse> ChangePassword(string currentPassword, string newPassword);
        Task<bool> Register(User user);
        Task<bool> ForgetPassword(string username);
        Task<bool> TouchLogin();
        void Logout();
    }
}
