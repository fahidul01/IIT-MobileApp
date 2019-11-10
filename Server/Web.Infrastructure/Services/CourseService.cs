using CoreEngine.Model.Common;
using CoreEngine.Model.DBModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<Course> AddCourse(Course course, int semesterId, int batchId)
        {
            var oldCourse = await _db.Courses
                                     .FirstOrDefaultAsync(x => x.CourseId == course.CourseId &&
                                                               x.Semester.Batch.Id == batchId);
            if (oldCourse != null)
            {
                return null;
            }
            else
            {
                var batch = await _db.Batches.FirstOrDefaultAsync(x => x.Id == batchId);
                var semester = await _db.Semesters.FirstOrDefaultAsync(x => x.Id == semesterId);
                if (batch == null || semester == null)
                {
                    return null;
                }
                else
                {
                    course.Semester = semester;
                    _db.Entry(course).State = EntityState.Added;
                    await _db.SaveChangesAsync();
                    return course;
                }
            }
        }

        public async Task<Course> GetCourseAsync(int id)
        {
            return await _db.Courses
                            .Include(x => x.Semester)
                            .Include(x => x.Lessons)
                            .Include(x => x.CourseMaterials)
                            .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<Lesson>> UpcomingLessons()
        {
            var today = CurrentTime.DayOfWeek;
            var dayLessons = await _db.Lessons
                                      .Where(x => x.Course.Semester.EndsOn >= CurrentTime &&
                                                  x.DayOfWeek == today)
                                      .Include(x => x.Course)
                                      .ThenInclude(x => x.Semester)
                                      .ThenInclude(x => x.Batch)
                                      .ToListAsync();
            return dayLessons;
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

        public async Task<Lesson> AddLesson(Lesson lesson, int courseId)
        {
            var course = await _db.Courses.FirstOrDefaultAsync(x => x.Id == courseId);
            if (course == null) return null;
            else
            {
                lesson.Course = course;
                _db.Entry(lesson).State = EntityState.Added;
                await _db.SaveChangesAsync();
                return lesson;
            }
        }
    }
}
