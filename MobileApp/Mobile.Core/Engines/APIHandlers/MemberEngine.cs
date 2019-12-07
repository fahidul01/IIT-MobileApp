using CoreEngine.APIHandlers;
using CoreEngine.Model.Common;
using CoreEngine.Model.DBModel;
using Mobile.Core.Worker;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Mobile.Core.Engines.APIHandlers
{
    public class MemberEngine : BaseEngine, IMemberHandler
    {
        public MemberEngine(HttpWorker httpWorker) : base(httpWorker, "member")
        {
        }

        public Task<ActionResponse> ChangePassword(string currentPassword, string newPassword)
        {
            return SendRequest<ActionResponse>(HttpMethod.Post, new { currentPassword, newPassword });
        }

        public Task<ActionResponse> DeleteUser(User user)
        {
            return SendRequest<ActionResponse>(HttpMethod.Get, user);
        }



        public Task<List<User>> GetCurrentBatchUsers()
        {
            return SendRequest<List<User>>(HttpMethod.Get, null);
        }

        public async Task<SignInResponse> Login(string username, string password)
        {
            var res = await SendRequest<SignInResponse>(HttpMethod.Post, new { username, password });
            if (res != null && res.Success)
            {
                _httpWorker.LoggedIn(res.Token);
            }
            return res;
        }

        public async void Logout()
        {
            await SendRequest<ActionResponse>(HttpMethod.Get, null);
            _httpWorker.Logout();
        }

        public Task<ActionResponse> Register(User user)
        {
            return SendRequest<ActionResponse>(HttpMethod.Post, user);
        }

        public Task<ActionResponse> UpdateUser(User user)
        {
            return SendRequest<ActionResponse>(HttpMethod.Post, user);
        }

        public Task<User> TouchLogin()
        {
            return SendRequest<User>(HttpMethod.Get, null);
        }

        public Task<ActionResponse> ForgetPassword(string username)
        {
            return SendRequest<ActionResponse>(HttpMethod.Post, new { username });
        }
    }
}
