using CoreEngine.APIHandlers;
using CoreEngine.Model.Common;
using CoreEngine.Model.DBModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Web.Infrastructure.Services;
using Web.WebServices;

namespace Web.Api
{
    [Authorize(Roles = AppConstants.Student,
       AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class MemberController : Controller, IMemberHandler
    {
        private readonly UserService _userService;
        private readonly SignInManager<DBUser> _signInmanager;
        private readonly UserManager<DBUser> _userManager;
        private readonly TokenService _tokenService;
        private readonly BatchService _batchService;

        public MemberController(
            UserService userService,
            SignInManager<DBUser> signInManager,
            UserManager<DBUser> userManager,
            TokenService tokenService,
            BatchService batchService)
        {
            _userService = userService;
            _signInmanager = signInManager;
            _userManager = userManager;
            _tokenService = tokenService;
            _batchService = batchService;
        }

        public async Task<ActionResponse> ChangePassword(string currentPassword, string newPassword)
        {
            if (HttpContext.User == null)
            {
                return new ActionResponse(false, "Invalid User");
            }

            var dbUser = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
            if (dbUser == null)
            {
                return new ActionResponse(false, "Invalid User");
            }
            else
            {
                var res = await _signInmanager.CheckPasswordSignInAsync(dbUser, currentPassword, false);
                if (res == Microsoft.AspNetCore.Identity.SignInResult.Success)
                {
                    var hashedPass = _userManager.PasswordHasher.HashPassword(dbUser, newPassword);
                    dbUser.PasswordHash = hashedPass;
                    var updateRes = await _userManager.UpdateAsync(dbUser);
                    return new ActionResponse(updateRes.Succeeded, "Password Updated");
                }
                else
                {
                    return new ActionResponse(false, "Failed to match Password");
                }
            }
        }

        public Task<ActionResponse> DeleteUser(User user)
        {
            return Task.FromResult(new ActionResponse(false, "Not Allowed"));
        }

        public async Task<ActionResponse> ForgetPassword(string username)
        {
            var res = await _userService.RecoverPassword(username);
            if (res)
            {
                return new ActionResponse(true, "A Password reset has been accepted. Please check your mail");
            }
            else
            {
                return new ActionResponse(false, "Failed to reset password. Try again");
            }
        }

        public async Task<List<User>> GetCurrentBatchUsers()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var batch = await _userService.GetBatch(userId);
            if (batch == null) return null;
            else return await _batchService.GetBatchStudents(batch.Id);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<SignInResponse> Login(string username, string password)
        {
            var res = await _signInmanager.PasswordSignInAsync(username, password, true, false);
            if (res.Succeeded)
            {
                var dbUser = await _userManager.FindByNameAsync(username);
                var token = _tokenService.GenerateJwtToken(username, dbUser);
                return new SignInResponse(true, token);
            }
            else
            {
                return new SignInResponse(false);
            }
        }

        public void Logout()
        {
            var user = HttpContext.User;
            if (user != null)
            {
                _signInmanager.SignOutAsync();
            }
        }

        public Task<ActionResponse> Register(User user)
        {
            return Task.FromResult(new ActionResponse(false, "Not Authorize"));
        }

        [HttpGet]
        [AllowAnonymous]
        public string Test()
        {
            return "Routing is ok";
        }

        public async Task<User> TouchLogin()
        {
            var user = HttpContext.User;
            var res = await _userManager.GetUserAsync(user);
            return CoreEngine.Model.DBModel.User.FromDBUser(res, "");
        }

        public async Task<ActionResponse> UpdateUser(User user)
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            if (currentUser?.Id == user.Id)
            {
                var res = await _userService.Update(user);
                if (res != null)
                {
                    return new ActionResponse(true, "User Update Successfull");
                }
            }
            return new ActionResponse(false, "Failed to update User");
        }
    }
}