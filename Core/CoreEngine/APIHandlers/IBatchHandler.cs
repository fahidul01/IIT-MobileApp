using CoreEngine.Model.DBModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreEngine.APIHandlers
{
    public interface IBatchHandler
    {
        Task<List<Batch>> GetBatchesAsync(int page = 1);
    }
}
