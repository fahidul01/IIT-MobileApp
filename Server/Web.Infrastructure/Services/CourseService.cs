using CoreEngine.Model.Common;
using CoreEngine.Model.DBModel;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.IO;
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

        #region Course
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
                var batch = await _db.Batches
                                     .Include(x => x.Students)
                                     .FirstOrDefaultAsync(x => x.Id == batchId);
                var semester = await _db.Semesters.FirstOrDefaultAsync(x => x.Id == semesterId);
                if (batch == null || semester == null)
                {
                    return null;
                }
                else
                {
                    foreach (var student in batch.Students)
                    {
                        var courseStudent = new StudentCourse()
                        {
                            Course = course,
                            Student = student,
                        };
                        course.StudentCourses.Add(courseStudent);
                    }
                    course.Semester = semester;
                    _db.Entry(course).State = EntityState.Added;
                    await _db.SaveChangesAsync();
                    return course;
                }
            }
        }

        public async Task<bool> Delete(int courseId, int batchId)
        {
            var course = await _db.Courses.Include(x => x.Semester)
                                    .ThenInclude(x => x.Batch)
                                    .FirstOrDefaultAsync(x => x.Id == courseId);
            if (course == null)
            {
                return false;
            }
            else
            {
                if (batchId == course.Semester.Batch.Id)
                {
                    _db.Entry(course).State = EntityState.Deleted;
                    await _db.SaveChangesAsync();
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public async Task<Course> GetCourseAsync(int id)
        {
            return await _db.Courses
                            .Include(x => x.StudentCourses)
                            .Include(x => x.Semester)
                            .Include(x => x.Lessons)
                            .Include(x => x.CourseMaterials)
                            .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<Course>> GetCoursesAsync(int batchId)
        {
            return await _db.Courses.Include(x => x.Semester)
                                    .Where(x => x.Semester.Batch.Id == batchId)
                                    .ToListAsync();
        }

        public async Task<bool> ModifyCourse(Course course)
        {
            var dbCourse = await _db.Courses.FindAsync(course.Id);
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

        #endregion

        #region Lesson

        public async Task<bool> UploadResult(int courseId, string filePath)
        {
            var course = await _db.Courses.Include(x => x.StudentCourses)
                                          .FirstOrDefaultAsync(x => x.Id == courseId);
            if (course == null)
            {
                return false;
            }

            using var stream = File.OpenRead(filePath);
            using var reader = new StreamReader(stream);
            while (!reader.EndOfStream)
            {
                var line = await reader.ReadLineAsync();
                var splitter = line.Split(',');
                var roll = splitter[0];
                var name = splitter[1];
                decimal.TryParse(splitter[2], out decimal gradePoint);
                var gradeName = splitter[3];

                var courseStudent = course.StudentCourses.FirstOrDefault(x => x.Student.UserName == roll);
                if (courseStudent == null)
                {
                    var student = await _db.Users.FirstOrDefaultAsync(x => x.UserName == roll);
                    if (student != null)
                    {
                        courseStudent = new StudentCourse()
                        {
                            Course = course,
                            GradePoint = gradePoint,
                            Grade = gradeName,
                            Student = student
                        };
                        _db.Entry(courseStudent).State = EntityState.Added;
                    }
                }
                else
                {
                    courseStudent.Grade = gradeName;
                    courseStudent.GradePoint = gradePoint;
                    _db.Entry(courseStudent).State = EntityState.Modified;
                }
            }
            await _db.SaveChangesAsync();
            return true;
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

        public async Task<ActionResponse> DeleteLesson(string userId, int lessonId)
        {
            var user = await _db.Users.Include(x => x.Batch)
                                       .FirstOrDefaultAsync(x => x.UserName == userId);
            if (user != null && user.Batch != null && user.ClassRepresentative)
            {
                var lesson = await _db.Lessons.FirstOrDefaultAsync(x => x.Id == lessonId);
                if (lesson != null && lesson.Course.StudentCourses.Any(x => x.Student.Id == userId))
                {
                    _db.Lessons.Remove(lesson);
                    await _db.SaveChangesAsync();
                }
            }
            return new ActionResponse(false, "Invalid Request");
        }

        public async Task<List<StudentCourse>> GetResult(string userId)
        {
            var res = await _db.StudentCourses.Include(x => x.Course)
                                              .ThenInclude(x => x.Semester)
                                              .Where(x => x.Student.Id == userId)
                                              .ToListAsync();
            return res;
        }

        #endregion


        #region Material
        public async Task<bool> AddMaterial(int courseId, List<DBFile> dbFiles)
        {
            var course = await _db.Courses.Include(x => x.CourseMaterials)
                                    .FirstOrDefaultAsync(x => x.Id == courseId);
            if (course == null)
            {
                return false;
            }
            else
            {
                dbFiles.ForEach(x => course.CourseMaterials.Add(x));
                await _db.SaveChangesAsync();
                return true;
            }
        }

        public async Task<List<Semester>> GetSemestersAsync(int batchId)
        {
            var res = await _db.Semesters
                               .Include(x => x.Courses)
                               .Where(x => x.Batch.Id == batchId)
                               .ToListAsync();
            return res;
        }

        public async Task<Lesson> AddUpdateLesson(int courseId, Lesson lesson)
        {
            var course = await _db.Courses.FirstOrDefaultAsync(x => x.Id == courseId);
            if (course == null)
            {
                return null;
            }
            else
            {
                if (lesson.Id == 0)
                {
                    lesson.Course = course;
                    _db.Entry(lesson).State = EntityState.Added;
                }
                else
                {
                    _db.Entry(lesson).State = EntityState.Modified;
                }
                await _db.SaveChangesAsync();
                return lesson;
            }
        }

        #endregion

    }
}
