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

        public async Task<IActionResult> Create(CreateCoursePopupModel createCoursePopupModel)
        {
            if (ModelState.IsValid)
            {
                var course = new Course()
                {
                    CourseCredit = createCoursePopupModel.CourseCredit,
                    CourseId = createCoursePopupModel.CourseId,
                    CourseName = createCoursePopupModel.CourseName
                };
                var newCourse = await _courseService.AddCourse(course, createCoursePopupModel.SemesterId, createCoursePopupModel.BatchId);
                if (newCourse != null)
                {
                    var res = await _courseService.GetSemesterAsync(createCoursePopupModel.SemesterId);
                    return PartialView("_Courses", res.Courses);
                }
            }
            return BadRequest();
        }
    }
}