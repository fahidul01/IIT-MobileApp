using CoreEngine.Model.DBModel;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Web.Infrastructure.DBModel;

namespace Web.Infrastructure.Services
{
    public class CourseService
    {
        private readonly StudentDBContext _db;

        public CourseService(StudentDBContext dBContext)
        {
            _db = dBContext;
        }

        public async Task<bool> AddCourse(Course course, int batchId)
        {
            var oldCourse = await _db.Courses
                                     .FirstOrDefaultAsync(x => x.Id == course.Id &&
                                                               x.Batch.Id == batchId);
            if (oldCourse != null) return false;
            else
            {
                var batch = await _db.Batches.FirstOrDefaultAsync(x => x.Id == batchId);
                if (batch == null) return false;
                else
                {
                    _db.Entry(course).State = EntityState.Added;
                    course.Batch = batch;
                    await _db.SaveChangesAsync();
                    return true;
                }
            }
        }

        public async Task<bool> ModifyCourse(Course course)
        {
            var dbCourse = await _db.Courses.FindAsync(course);
            if (dbCourse == null)
            {
                return false;
            }
            else
            {
                _db.Entry(dbCourse).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                return true;
            }
        }
    }
}
