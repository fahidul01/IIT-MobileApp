using CoreEngine.APIHandlers;
using CoreEngine.Engine;
using CoreEngine.Model.Common;
using CoreEngine.Model.DBModel;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace CoreEngine.APIEngines
{
    class MemberEngine : BaseApiEngine, IMemberHandler
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
                LoginToken(res.Token);
            }
            return res;
        }



        public async void Logout()
        {
            await SendRequest<ActionResponse>(HttpMethod.Get, null);
            LogoutToken();
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

        public Task<ActionResponse> CreateStudent(int batchId, string roll, string name, string email, string phone)
        {
            return SendRequest<ActionResponse>(HttpMethod.Post, new { batchId, roll, name, email, phone });
        }

        public Task<ActionResponse> CreateBatchStudents(int batchId, DBFile dBFile, IFormFile formFile = null)
        {
            return SendMultiPartRequest<ActionResponse>(new { batchId }, dBFile);
        }

        public Task<List<User>> SearchStudents(string key)
        {
            return SendRequest<List<User>>(HttpMethod.Get, new { key });
        }
    }
}
