using CoreEngine.Model.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Web.Areas.Admin.ViewModels;
using Web.Controllers;
using Student.Infrastructure.Services;

namespace Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = AppConstants.Admin)]
    public class DashboardController : BaseController
    {
        private readonly CourseService _courseService;
        private readonly NoticeService _noticeService;
        private readonly UserService _userService;

        public DashboardController(CourseService courseService, NoticeService noticeService, UserService userService)
        {
            _courseService = courseService;
            _noticeService = noticeService;
            _userService = userService;
        }
        public async Task<IActionResult> Index()
        {
            //var recent = await _noticeService.GetRecentNotice(1);
            var notices = await _noticeService.GetUpcomingEvents();
            var batch = await _userService.GetCurrentCr();
            var lessons = await _courseService.UpcomingLessons();
            return View(new DashboardViewModel(lessons, notices, batch));
        }
    }
}