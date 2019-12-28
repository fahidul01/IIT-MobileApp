using CoreEngine.Model.Common;
using CoreEngine.Model.DBModel;
using Microsoft.EntityFrameworkCore;
using Student.Infrastructure.DBModel;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Student.Infrastructure.Services
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

        public async Task<Batch> AddBatch(Batch batch)
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
                return batch;
            }
            else
            {
                return batch;
            }
        }

        public async Task<Batch> GetBatchAsync(int id)
        {
            var res = await _db.Batches
                               .Include(x => x.Students)
                               .Include(x => x.Semesters)
                               .ThenInclude(x => x.Courses)
                               .FirstOrDefaultAsync(x => x.Id == id);
            return res;
        }

        public async Task<ActionResponse> UpdateBatch(Batch batch)
        {
            var oldBatch = await _db.Batches.FindAsync(batch.Id);
            if (oldBatch == null)
            {
                return new ActionResponse(false, "The batch information was not found");
            }
            else
            {
                _db.Entry(batch).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                return new ActionResponse(true, "Batch Information Updated Successfully");
            }
        }

        public async Task<ActionResponse> DeleteBatch(Batch batch)
        {
            var oldBatch = await _db.Batches.Include(x => x.Students)
                                            .Include(x => x.Semesters)
                                            .FirstOrDefaultAsync(x => x.Id == batch.Id);
            if (oldBatch == null)
            {
                return new ActionResponse(false, "The batch information was not found");
            }
            else
            {
                foreach (var student in oldBatch.Students)
                {
                    _db.Entry(student).State = EntityState.Deleted;
                }

                foreach (var semester in oldBatch.Students)
                {
                    _db.Entry(semester).State = EntityState.Deleted;
                }

                _db.Entry(oldBatch).State = EntityState.Deleted;
                await _db.SaveChangesAsync();
                return new ActionResponse(true, "Batch Information Deleted Successfully");
            }
        }

        public async Task<List<DBUser>> GetBatchStudents(int batchId)
        {
            var batch = await _db.Batches
                                 .Include(x => x.Students)
                                 .FirstOrDefaultAsync(x => x.Id == batchId);
            return batch.Students.ToList();
        }
    }
}
