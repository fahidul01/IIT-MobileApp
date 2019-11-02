using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Web.Infrastructure.Services;

namespace Web.Areas.Admin.ViewComponents
{
    public class CoursesViewComponent : ViewComponent
    {
        private readonly BatchService _batchService;

        public CoursesViewComponent(BatchService batchService)
        {
            _batchService = batchService;
        }

        public async Task<IViewComponentResult> InvokeAsync(int semesterId)
        {
            var items = await _batchService.GetCoursesAsync(semesterId);
            return View(items);
        }
    }
}
