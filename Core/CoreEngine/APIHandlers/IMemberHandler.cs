using CoreEngine.Model.DBModel;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace CoreEngine.APIHandlers
{
    public interface IMemberHandler
    {
        Task<SignInResult> Login(string username, string password);
        Task<IdentityResult> ChangePassword(string currentPassword, string newPassword);
        Task<bool> Register(User user);
        Task<bool> ForgetPassword(string username);
    }
}
