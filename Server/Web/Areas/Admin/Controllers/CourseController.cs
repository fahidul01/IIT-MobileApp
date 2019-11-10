using CoreEngine.Model.DBModel;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Web.Areas.Admin.ViewModels;
using Web.Controllers;
using Web.Infrastructure.Services;

namespace Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CourseController : BaseController
    {
        private readonly CourseService _courseService;

        public CourseController(CourseService course)
        {
            _courseService = course;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Details(int id)
        {
            var course = await _courseService.GetCourseAsync(id);
            return View(course);
        }
    }
}