using CoreEngine.Model.Common;
using CoreEngine.Model.DBModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using Web.Areas.Admin.ViewModels;
using Web.Infrastructure.Services;

namespace Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class NoticesController : Controller
    {
        private readonly UserManager<DBUser> _usermanager;
        private readonly BatchService _batchService;
        private readonly NoticeService _noticeService;

        public NoticesController(NoticeService noticeService,
            BatchService batchService,
            UserManager<DBUser> userManager)
        {
            _usermanager = userManager;
            _batchService = batchService;
            _noticeService = noticeService;
        }

        // GET: Admin/Notices
        public async Task<IActionResult> Index()
        {
            ViewData["CurrentPage"] = 1;
            ViewData["TotalPage"] = Math.Max(1, await _noticeService.GetTotalNoticeAsync() / 20);
            var uNotice = await _noticeService.GetUpcomingEvents();
            var rNotice = await _noticeService.GetRecentNotice(1);
            return View(new IndexNoticeViewModel(uNotice, rNotice));
        }

        public async Task<IActionResult> RecentNoticepage(int page)
        {
            var rNotice = await _noticeService.GetRecentNotice(page);
            return PartialView("_Notices", rNotice);
        }

        // GET: Admin/Notices/Create
        public async Task<IActionResult> Create()
        {
            var currentBatch = await _batchService.GetBatchesAsync(1);
            return View(new CreateNoticeViewModel(currentBatch));
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateNoticeViewModel noticeViewModel)
        {
            if (ModelState.IsValid)
            {
                var owners = await _usermanager.GetUsersInRoleAsync(AppConstants.Admin);
                var owner = owners.FirstOrDefault();
                var batch = noticeViewModel.IsAllBatch ? 0 : noticeViewModel.BatchId;
                var notice = new Notice()
                {
                    EventDate = noticeViewModel.EventDate,
                    FutureNotification = noticeViewModel.EventDate.Date > DateTime.Now.Date,
                    Message = noticeViewModel.Message,
                    Title = noticeViewModel.Title,
                    PostType = noticeViewModel.PostType,
                };
                await _noticeService.AddNotice(notice, owner, batch);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(noticeViewModel);
            }
        }
        // GET: Admin/Notices/Edit/5
        public IActionResult Edit(int? id)
        {
            return View();
        }
    }
}
