﻿using CoreEngine.Model.Common;
using CoreEngine.Model.DBModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Web.Controllers;
using Web.Infrastructure.Services;

namespace Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = AppConstants.Admin)]
    public class BatchController : BaseController
    {
        private readonly BatchService _batchService;
        private readonly UserService _userService;

        public BatchController(BatchService batchService, UserService userService)
        {
            _batchService = batchService;
            _userService = userService;
        }
        public async Task<IActionResult> Index(int page = 1)
        {
            ViewData["CurrentPage"] = page;
            ViewData["TotalPage"] = Math.Max(1, await _batchService.GetCount() / 10);
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
                if (res == null)
                {
                    Failed("Failed to update");
                    return View(batch);
                }
                else
                {
                    return View(nameof(Details), batch);
                }
            }
            else
            {
                return View(batch);
            }
        }
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var batch = await _batchService.GetBatchAsync(id);
            return View(batch);
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
                if (res.Actionstatus)
                {
                    Success();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    Failed(res.Message);
                    return View(batch);
                }
            }
            else
            {
                return View(batch);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateStudent(int batchId, string roll, string name, string email, string phone)
        {
            if (batchId == 0)
            {
                Failed("Invalid Batch");
            }
            else if (string.IsNullOrWhiteSpace(roll))
            {
                Failed("Invalid Roll");
            }
            else if (string.IsNullOrWhiteSpace(name))
            {
                Failed("Invalid Name");
            }
            else if (string.IsNullOrWhiteSpace(email))
            {
                Failed("Invalid Email Address");
            }
            else
            {
                var res = await _userService.AddStudent(batchId, roll, name, email, phone);
                if (res.Actionstatus)
                {
                    return PartialView("_Students", await _batchService.GetBatchStudents(batchId));
                }
                else
                {
                    Failed(res.Message);
                }
            }
            return PartialView("_Students", new List<User>());
        }

        [HttpPost]
        public async Task<IActionResult> CreateStudents(int id, IFormFile file)
        {
            if (id > 0 && file != null && file.Length > 0)
            {
                var filePath = Path.GetTempFileName();

                try
                {
                    using (var stream = System.IO.File.Create(filePath))
                    {
                        await file.CopyToAsync(stream);
                    }

                    if (file.FileName.EndsWith("csv"))
                    {
                        var res = await _userService.UploadCSVStudents(filePath, id);
                        return PartialView("_Students", res);
                    }
                    else
                    {
                        Failed("Invalid File format");
                        return PartialView("_Students", new List<User>());
                    }
                }
                catch (Exception ex)
                {
                    Failed(ex.Message);
                    return PartialView("_Students", new List<User>());
                }
            }
            else
            {
                Failed("Invalid File format");
                return PartialView("_Students", new List<User>());
            }
        }
    }
}