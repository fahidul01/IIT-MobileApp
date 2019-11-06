using CoreEngine.Model.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Web.Controllers;
using Web.Infrastructure.Services;

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
            var recent = await _noticeService.GetRecentNotice(1);
            var upcoming = await _noticeService.GetUpcomingEvents();
            var batch = await _userService.GetCurrentCr();
            return View();
        }
    }
}