using CoreEngine.Model.Common;
using CoreEngine.Model.DBModel;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Web.Infrastructure.DBModel;

namespace Web.Infrastructure.Services
{
    public class CourseService : BaseService
    {
        private readonly StudentDBContext _db;

        public CourseService(StudentDBContext dBContext)
        {
            _db = dBContext;
        }

        public async Task<bool> AddCourse(Course course, int semesterId, int batchId)
        {
            var oldCourse = await _db.Courses
                                     .FirstOrDefaultAsync(x => x.CourseId == course.CourseId &&
                                                               x.Semester.Batch.Id == batchId);
            if (oldCourse != null) return false;
            else
            {
                var batch = await _db.Batches.FirstOrDefaultAsync(x => x.Id == batchId);
                var semester = await _db.Semesters.FirstOrDefaultAsync(x => x.Id == semesterId);
                if (batch == null || semester == null) return false;
                else
                {
                    course.Semester = semester;
                    _db.Entry(course).State = EntityState.Added;
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

        public async Task<Semester> GetSemesterAsync(int semesterId)
        {
            var semester = await _db.Semesters
                                    .Include(m => m.Batch)
                                    .Include(x => x.Courses)
                                    .FirstOrDefaultAsync(x => x.Id == semesterId);
            return semester;
        }
    }
}
