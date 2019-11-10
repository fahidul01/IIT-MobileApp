using CoreEngine.APIHandlers;
using CoreEngine.Model.DBModel;
using Microsoft.AspNetCore.Identity;
using Mobile.Core.Worker;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Mobile.Core.Engines.APIHandlers
{
    public class MemberEngine : BaseEngine, IMemberHandler
    {
        public MemberEngine(HttpWorker httpWorker) : base(httpWorker, "member")
        {
        }

        public Task<IdentityResult> ChangePassword(string currentPassword, string newPassword)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ForgetPassword(string username)
        {
            throw new NotImplementedException();
        }

        public Task<SignInResult> Login(string username, string password)
        {
            return SendRequest<SignInResult>(HttpMethod.Post, new { username, password });
        }

        public Task<bool> Register(User user)
        {
            return SendBoolRequest(HttpMethod.Post, user);
        }
    }
}
