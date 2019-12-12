using CoreEngine.Model.Common;
using CoreEngine.Model.DBModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<DBUser> _usermanager;

        public HomeController(UserManager<DBUser> userManager)
        {
            _usermanager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _usermanager.GetUserAsync(HttpContext.User);
            if (user == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                var adminRole = await _usermanager.IsInRoleAsync(user, AppConstants.Admin);

                if (adminRole)
                {
                    return RedirectToAction("index", "Dashboard", new { area = "admin" });
                }
                else
                {
                    return View();
                }
            }
        }
    }
}
