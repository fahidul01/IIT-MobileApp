using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Web.Infrastructure.Services;

namespace Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class StudentController : Controller
    {
        private UserService _userService;

        public StudentController(UserService userService)
        {
            _userService = userService;
        }
        // GET: Student
        public ActionResult Index()
        {
            return View();
        }

        // GET: Student/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> SearchStudents(string value = "")
        {
            var students = await _userService.SearchStudent(value);
            return PartialView("_Students", students);
        }
    }
}