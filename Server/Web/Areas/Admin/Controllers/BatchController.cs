using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreEngine.Model.DBModel;
using Microsoft.AspNetCore.Mvc;
using Web.Infrastructure.Services;

namespace Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BatchController : Controller
    {
        private readonly BatchService _batchService;

        public BatchController(BatchService batchService)
        {
            _batchService = batchService;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }
    }
}