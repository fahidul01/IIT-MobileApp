using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Web.Infrastructure.Services;

namespace Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class StudentController : Controller
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
            var student = await _userService.GetStudent(id);
            if (student == null) return NotFound();
            return View(student);
        }

        [HttpPost]
        public async Task<ActionResult> SearchStudents(string value = "")
        {
            var students = await _userService.SearchStudent(value);
            return PartialView("_Students", students);
        }
    }
}