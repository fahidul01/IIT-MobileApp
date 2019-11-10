using CoreEngine.APIHandlers;
using CoreEngine.Model.Common;
using CoreEngine.Model.DBModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Web.WebServices;

namespace Web.Api
{
    //[Route("api/[controller]")]
    //[ApiController]
    public class MemberController : Controller, IMemberHandler
    {
        private readonly SignInManager<DBUser> _signInmanager;
        private readonly UserManager<DBUser> _userManager;
        private readonly TokenService _tokenService;

        public MemberController(
            SignInManager<DBUser> signInManager,
            UserManager<DBUser> userManager,
            TokenService tokenService)
        {
            _signInmanager = signInManager;
            _userManager = userManager;
            _tokenService = tokenService;
        }

        [Authorize]
        public async Task<SignInResponse> ChangePassword(string currentPassword, string newPassword)
        {
            if (HttpContext.User == null)
            {
                return new SignInResponse(false);
            }

            var dbUser = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
            if (dbUser == null)
            {
                return new SignInResponse(false);
            }
            else
            {
                var res = await _signInmanager.CheckPasswordSignInAsync(dbUser, currentPassword, false);
                if (res == Microsoft.AspNetCore.Identity.SignInResult.Success)
                {
                    var hashedPass = _userManager.PasswordHasher.HashPassword(dbUser, newPassword);
                    dbUser.PasswordHash = hashedPass;
                    var updateRes = await _userManager.UpdateAsync(dbUser);
                    return new SignInResponse(updateRes.Succeeded);
                }
                else
                {
                    return new SignInResponse(false);
                }
            }
        }

        public Task<bool> ForgetPassword(string username)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public async Task<SignInResponse> Login(string username, string password)
        {
            var res = await _signInmanager.PasswordSignInAsync(username, password, true, false);
            if (res.Succeeded)
            {
                var dbUser = await _userManager.FindByNameAsync(username);
                var token = _tokenService.GenerateJwtToken(username, dbUser);
                return new SignInResponse(true, token);
            }
            else return new SignInResponse(false);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public void Logout()
        {
            var user = HttpContext.User;
            if (user != null)
            {
                _signInmanager.SignOutAsync();
            }
        }

        public Task<bool> Register(User user)
        {
            return Task.FromResult(false);
        }

        [HttpGet]
        public string Test()
        {
           return "Routing is ok";
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public Task<bool> TouchLogin()
        {
            var user = HttpContext.User;
            return Task.FromResult(true);
        }
    }
}