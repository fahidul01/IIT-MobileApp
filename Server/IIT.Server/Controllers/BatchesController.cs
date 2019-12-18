using CoreEngine.APIHandlers;
using CoreEngine.Model.DBModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Student.Infrastructure.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IIT.Server.Controllers
{
    [Authorize]
    public class BatchesController : ControllerBase, IBatchHandler
    {
        private readonly BatchService _batchService;

        public BatchesController(BatchService batchService)
        {
            _batchService = batchService;
        }

        public async Task<List<Batch>> GetBatches(int page = 1)
        {
            return await _batchService.GetBatchesAsync(page);
        }

        public string Test()
        {
            return "Batch Working";
        }
    }
}