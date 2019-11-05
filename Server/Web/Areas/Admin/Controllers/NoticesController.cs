using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Web.Infrastructure.Services;
using System;
using Web.Areas.Admin.ViewModels;
using CoreEngine.Model.DBModel;

namespace Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class NoticesController : Controller
    {
        private readonly BatchService _batchService;
        private readonly NoticeService _noticeService;

        public NoticesController(NoticeService noticeService,
            BatchService batchService)
        {
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
            var currentBatches = await _batchService.GetBatchesAsync(1);
            return View(new CreateNoticeViewModel(currentBatches));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateNoticeViewModel createNoticeViewModel)
        {
            if (ModelState.IsValid)
            {
                var notice = new Notice()
                {
                    EventDate = createNoticeViewModel.EventDate,
                    Title = createNoticeViewModel.Title,
                    PostType = createNoticeViewModel.PostType
                };
                var res = await _noticeService.AddNotice(notice, null);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(createNoticeViewModel);
            }
        }


        // GET: Admin/Notices/Edit/5
        public IActionResult Edit(int? id)
        {
            return View();
        }
    }
} 
