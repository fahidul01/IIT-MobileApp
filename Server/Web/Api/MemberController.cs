using System;
using System.Threading.Tasks;
using CoreEngine.APIHandlers;
using CoreEngine.Model.DBModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Web.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class MemberController : Controller, IMemberHandler
    {
        private readonly SignInManager<DBUser> _signInmanager;
        private readonly UserManager<DBUser> _userManager;

        public MemberController(
            SignInManager<DBUser> signInManager,
            UserManager<DBUser> userManager)
        {
            _signInmanager = signInManager;
            _userManager = userManager;
        }

        [Authorize]
        public async Task<IdentityResult> ChangePassword(string currentPassword, string newPassword)
        {
            if (HttpContext.User == null) return IdentityResult.Failed();
            var dbUser = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
            if (dbUser == null) return IdentityResult.Failed();
            else
            {
                var res = await _signInmanager.CheckPasswordSignInAsync(dbUser, currentPassword, false);
                if (res == Microsoft.AspNetCore.Identity.SignInResult.Success)
                {
                    var hashedPass = _userManager.PasswordHasher.HashPassword(dbUser, newPassword);
                    dbUser.PasswordHash = hashedPass;
                    return await _userManager.UpdateAsync(dbUser);
                }
                else return IdentityResult.Failed();
            }
        }

        public Task<bool> ForgetPassword(string username)
        {
            throw new NotImplementedException();
        }

        public Task<Microsoft.AspNetCore.Identity.SignInResult> Login(string username, string password)
        {
            return _signInmanager.PasswordSignInAsync(username, password, true, false);
        }

        public Task<bool> Register(User user)
        {
            return Task.FromResult(false);
        }
    }
}