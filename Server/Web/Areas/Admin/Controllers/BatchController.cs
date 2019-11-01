using System.Threading.Tasks;
using CoreEngine.Model.DBModel;
using Microsoft.AspNetCore.Mvc;
using Web.Controllers;
using Web.Infrastructure.Services;

namespace Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BatchController : BaseController
    {
        private readonly BatchService _batchService;

        public BatchController(BatchService batchService)
        {
            _batchService = batchService;
        }
        public async Task<IActionResult> Index(int page = 1)
        {
            ViewData["CurrentPage"] = page;
            ViewData["TotalPage"] = await _batchService.GetCount();
            return View(await _batchService.GetBatchesAsync(page));
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Batch batch)
        {
            if (ModelState.IsValid)
            {
                var res = await _batchService.AddBatch(batch);
                if (res)
                {
                    Success();
                    return RedirectToAction(nameof(Edit), batch.Id);
                }
                else
                {
                    Failed("Failed to update");
                    return View(batch);
                }
            }
            else return View(batch);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var batch = await _batchService.GetBatchAsync(id.Value);
            return View(batch);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Batch batch)
        {
            if (ModelState.IsValid)
            {
                var res = await _batchService.UpdateBatch(batch);
                if (res)
                {
                    Success();
                    return RedirectToAction(nameof(Edit), batch.Id);
                }
                else
                {
                    Failed("Failed to update");
                    return View(batch);
                }
            }
            else return View(batch);
        }
    }
}