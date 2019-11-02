using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Web.Infrastructure.Services;

namespace Web.Areas.Admin.ViewComponents
{
    public class CoursesViewComponent : ViewComponent
    {
        private readonly CourseService _couseService;

        public CoursesViewComponent(CourseService course)
        {
            _couseService = course;
        }

        public async Task<IViewComponentResult> InvokeAsync(int semesterId)
        {
            var items = await _couseService.GetSemesterAsync(semesterId);
            return View(items);
        }
    }
}
