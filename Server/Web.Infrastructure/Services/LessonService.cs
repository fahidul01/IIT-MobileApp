using CoreEngine.Model.Common;
using CoreEngine.Model.DBModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Infrastructure.DBModel;

namespace Web.Infrastructure.Services
{
    public class LessonService : BaseService
    {
        private readonly StudentDBContext _db;
        public LessonService(StudentDBContext studentDBContext)
        {
            _db = studentDBContext;
        }

        public async Task<Lesson> AddLesson(int courseId, int batchId, Lesson lesson)
        {
            var course = await _db.Courses
                                 .FirstOrDefaultAsync(x => x.Id == courseId &&
                                 x.Semester.Batch.Id == batchId);
            if (course == null) return null;
            else
            {
                course.Lessons.Add(lesson);
                await _db.SaveChangesAsync();
                return lesson;
            }
        }

        public async Task<List<Lesson>> GetLesson(string userId)
        {
            var allCourse = _db.StudentCourses.Where(x => x.Student.Id == userId)
                                              .Select(m => m.Course);
            var lessons = await allCourse.Where(x => x.Semester.StartsOn < CurrentTime && x.Semester.EndsOn >= CurrentTime)
                                         .SelectMany(x => x.Lessons).Distinct()
                                         .ToListAsync();
            return lessons;
        }

        public async Task<Lesson> UpdateLesson(int batchId, Lesson lesson)
        {
            var course = _db.Lessons
                            .FirstOrDefaultAsync(x => x.Id == lesson.Id &&
                                                 x.Course.Semester.Batch.Id == batchId);
            if (course == null) return null;
            else
            {
                _db.Entry(lesson).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                return lesson;
            }
        }
    }
}
