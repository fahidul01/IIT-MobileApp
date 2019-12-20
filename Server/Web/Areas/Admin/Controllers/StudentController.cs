using CoreEngine.Model.Common;
using CoreEngine.Model.DBModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Web.Controllers;
using Student.Infrastructure.Services;

namespace Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = AppConstants.Admin)]
    public class StudentController : BaseController
    {
        private readonly UserService _userService;

        public StudentController(UserService userService)
        {
            _userService = userService;
        }
        // GET: Student
        public async Task<IActionResult> Index()
        {
            var students = await _userService.SearchStudent("");
            return View(students);
        }

        // GET: Student/Details/5
        public async Task<IActionResult> Details(string id)
        {
            var student = await _userService.GetUser(id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        [HttpPost]
        public async Task<IActionResult> Update(User user)
        {
            var student = await _userService.Update(user);
            if (student == null)
            {
                return NotFound();
            }

            return View(nameof(Details), student);
        }

        [HttpPost]
        public async Task<ActionResult> SearchStudents(string value = "")
        {
            var students = await _userService.SearchStudent(value);
            return PartialView("_Students", students);
        }

        public async Task<IActionResult> MakeCR(string id)
        {
            var user = await _userService.MakeCR(id);
            if (user == null)
            {
                return NotFound();
            }

            return (View(nameof(Details), user));
        }

        public async Task<IActionResult> RemoveCR(string id)
        {
            var user = await _userService.RemoveCR(id);
            if (user == null)
            {
                return NotFound();
            }

            return (View(nameof(Details), user));
        }

        [HttpGet]
        public async Task<IActionResult> RecoverPassword(string id)
        {
            var res = await _userService.RecoverPassword(id);
            if (res)
            {
                SuccessMessage = "Updated Password Successfully";
            }
            else
            {
                SuccessMessage = "Failed to update";
            }

            return PartialView("_Message");
        }

    }
}