﻿using System.Collections.Generic;
using System.Threading.Tasks;
using CoreEngine.APIHandlers;
using CoreEngine.Model.DBModel;
using Microsoft.AspNetCore.Mvc;
using Web.Infrastructure.Services;

namespace IIT.Web.Controllers
{
    public class BatchesController : ControllerBase,IBatchHandler
    {
        private readonly BatchService _batchService;

        public BatchesController(BatchService batchService)
        {
            _batchService = batchService;
        }

        public async Task<List<Batch>> GetBatchesAsync(int page = 1)
        {
            return await _batchService.GetBatchesAsync(page);
        }
    }
}