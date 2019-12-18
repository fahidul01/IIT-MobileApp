using CoreEngine.APIHandlers;
using CoreEngine.Engine;
using CoreEngine.Model.DBModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreEngine.APIEngines
{
    public class BatchEngine : IBatchHandler
    {
        private readonly IHttpWorker _httpWorker;
        private const string controllerName = "/api/batches/";

        public BatchEngine(IHttpWorker httpWorker)
        {
            _httpWorker = httpWorker;
        }

        public Task<List<Batch>> GetBatches(int page = 1)
        {
            var path = controllerName + "GetBatches" + "?page=" + page;
            Console.WriteLine(path);
            return _httpWorker.GetJsonAsync<List<Batch>>(path);
        }
    }
}
