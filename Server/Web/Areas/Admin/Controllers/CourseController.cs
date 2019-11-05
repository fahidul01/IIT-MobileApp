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

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCoursePopupModel popupModel)
        {
            if (ModelState.IsValid)
            {
                var course = new Course()
                {
                    CourseCredit = popupModel.CourseCredit,
                    CourseId = popupModel.CourseId,
                    CourseName = popupModel.CourseName
                };
                var res = await _courseService.AddCourse(course, popupModel.SemesterId, popupModel.BatchId);
                if (!res)
                {
                    Failed("Failed to Update Course");
                }

                return ViewComponent("Courses", popupModel.SemesterId);
            }
            else
            {
                Failed("Failed to update Course");
                return ViewComponent("Courses", popupModel.SemesterId);
            }
        }
    }
}