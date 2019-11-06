using CoreEngine.Model.Common;
using CoreEngine.Model.DBModel;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
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
        public async Task<List<Batch>> GetBatchesAsync(int currentPage, int pageCount = 10)
        {
            var start = (currentPage - 1) * pageCount;
            return await _db.Batches.OrderByDescending(x => x.StartsOn)
                                    .Skip(start)
                                    .Take(pageCount)
                                    .ToListAsync();
        }

        public async Task<int> GetCount()
        {
            return await _db.Batches.CountAsync();
        }

        public async Task<bool> AddBatch(Batch batch)
        {
            if (batch.Id == 0)
            {
                for (int counter = 0; counter < batch.NumberOfSemester; counter++)
                {
                    var sem = new Semester()
                    {
                        Duration = batch.SemesterDuration,
                        Name = "Semester " + (counter + 1).ToString(),
                        StartsOn = batch.StartsOn.AddMonths(counter * batch.SemesterDuration),
                        EndsOn = batch.StartsOn.AddMonths((counter + 1) * batch.SemesterDuration).AddMinutes(-1)
                    };
                    batch.Semesters.Add(sem);
                }
                batch.EndsOn = batch.StartsOn
                                    .AddMonths(batch.SemesterDuration * batch.NumberOfSemester)
                                    .AddDays(7);
                _db.Batches.Add(batch);
                await _db.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<Batch> GetBatchAsync(int id)
        {
            var res = await _db.Batches
                               .Include(x => x.Students)
                               .Include(x => x.Semesters)
                               .FirstOrDefaultAsync(x => x.Id == id);
            res.LoadUsers();
            return res;
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
