using System.Threading.Tasks;
using CoreEngine.Model.DBModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class LoginController : Controller
    {
        private readonly SignInManager<DBUser> _signInManager;

        public LoginController(SignInManager<DBUser> signInManager)
        {
            _signInManager = signInManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string userName, string password, bool isPersistent = true)
        {
            var res = await _signInManager.PasswordSignInAsync(userName, password, isPersistent, false);
            if (res == Microsoft.AspNetCore.Identity.SignInResult.Success)
            {
                return RedirectToAction(nameof(Index), "Home");
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}