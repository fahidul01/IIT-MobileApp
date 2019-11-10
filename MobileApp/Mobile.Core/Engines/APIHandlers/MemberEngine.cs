using CoreEngine.APIHandlers;
using CoreEngine.Model.Common;
using CoreEngine.Model.DBModel;
using Mobile.Core.Worker;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Mobile.Core.Engines.APIHandlers
{
    public class MemberEngine : BaseEngine, IMemberHandler
    {
        public MemberEngine(HttpWorker httpWorker) : base(httpWorker, "member")
        {
        }

        public Task<SignInResponse> ChangePassword(string currentPassword, string newPassword)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ForgetPassword(string username)
        {
            throw new NotImplementedException();
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
            await SendBoolRequest(HttpMethod.Get, null);
            _httpWorker.Logout();
        }

        public Task<bool> Register(User user)
        {
            return SendBoolRequest(HttpMethod.Post, user);
        }

        public Task<bool> TouchLogin()
        {
            return SendBoolRequest(HttpMethod.Get, null);
        }
    }
}
