using CoreEngine.Model.Common;
using CoreEngine.Model.DBModel;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Web.Infrastructure.DBModel;

namespace Web.Infrastructure.Services
{
    public class BatchService : BaseService
    {
        private readonly StudentDBContext _db;

        public BatchService(StudentDBContext studentDBContext)
        {
            _db = studentDBContext;
        }

        public async Task<bool> AddBatch(Batch batch)
        {
            var oldBatch = await _db.Batches.FindAsync(batch);
            if (oldBatch == null)
            {
                _db.Batches.Add(batch);
                await _db.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> UpdateBatch(Batch batch)
        {
            var oldBatch = await _db.Batches.FindAsync(batch);
            if (oldBatch == null)
            {
                return false;
            }
            else
            {
                _db.Entry(batch).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                return true;
            }
        }

        public async Task<bool> DeleteBatch(Batch batch)
        {
            var oldBatch = await _db.Batches.FindAsync(batch);
            if (oldBatch == null)
            {
                return false;
            }
            else
            {
                _db.Entry(oldBatch).State = EntityState.Deleted;
                await _db.SaveChangesAsync();
                return true;
            }
        }
    }
}
