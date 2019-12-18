using System.Collections.Generic;
using System.Threading.Tasks;
using CoreEngine.APIHandlers;
using CoreEngine.Model.DBModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Student.Infrastructure.Services;

namespace IIT.Web.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class BatchesController : ControllerBase,IBatchHandler
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
    }
}