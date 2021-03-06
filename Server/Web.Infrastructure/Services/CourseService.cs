﻿using CoreEngine.Model.Common;
using CoreEngine.Model.DBModel;
using Microsoft.EntityFrameworkCore;
using Student.Infrastructure.AppServices;
using Student.Infrastructure.DBModel;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Student.Infrastructure.Services
{
    public class CourseService : BaseService
    {
        private const string RollColumn = "roll";
        private const string PointColumn = "point";
        private const string GradeColumn = "grade";
        private readonly StudentDBContext _db;
        private readonly INotificationService _notificationService;

        public CourseService(StudentDBContext dBContext, INotificationService notificationService)
        {
            _db = dBContext;
            _notificationService = notificationService;
        }

        #region Course
        public async Task<ActionResponse> AddCourse(Course course, int semesterId, int batchId)
        {
            course.CourseId = course.CourseId.ToUpper();
            var oldCourse = await _db.Courses
                                     .FirstOrDefaultAsync(x => x.CourseId == course.CourseId &&
                                                               x.Semester.Batch.Id == batchId);
            if (oldCourse != null)
            {
                return new ActionResponse(false, "Course Information already exists");
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
                        _db.StudentCourses.Add(courseStudent);
                    }
                    course.Semester = semester;
                    _db.Courses.Add(course);
                    await _db.SaveChangesAsync();
                    var message = string.Format("You have been enrolled in Course {0}", course.CourseName);
                    _notificationService.SendNotification(batch.Name, course.CourseName, message);
                    return new ActionResponse(true)
                    {
                        Data = course.Id
                    };
                }
            }
        }

        public async Task<ActionResponse> DeleteStudentCourse(int studentCourseId)
        {
            var res = await _db.StudentCourses.Include(x => x.Course)
                                              .FirstOrDefaultAsync(x => x.Id == studentCourseId);
            if (res == null)
            {
                return new ActionResponse(false, "Failed to Locate Student");
            }
            else
            {
                var course = res.Course;
                _db.Entry(res).State = EntityState.Deleted;
                await _db.SaveChangesAsync();
                return new ActionResponse(true)
                {
                    Data = course
                };
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
                            .Include(x => x.Semester)
                            .Include(x => x.Lessons)
                            .Include(x => x.CourseMaterials)
                            .Include(x => x.StudentCourses)
                            .ThenInclude(n => n.Student)
                            .FirstOrDefaultAsync(x => x.Id == id);
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
                _db.Entry(dbCourse).State = EntityState.Detached;
                _db.Entry(course).State = EntityState.Modified;
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
        public async Task<ActionResponse> UploadResult(int courseId, string filePath)
        {
            var course = await _db.Courses.Include(x => x.Semester.Batch)
                                          .FirstOrDefaultAsync(x => x.Id == courseId);
            return await AddResult(course, course.Semester.Batch, filePath);
        }

        public async Task<List<StudentCourse>> GetResult(int courseId)
        {
            var res = await _db.StudentCourses.Where(x => x.Course.Id == courseId)
                               .Include(m => m.Student)
                               .ToListAsync();
            return res;
        }

        private async Task<ActionResponse> AddResult(Course course, Batch batch, string filePath)
        {
            using var stream = File.OpenRead(filePath);
            using var reader = new StreamReader(stream);

            int rollCol = 0, gradePointCol = 1, gradeCol = 2;
            var lineNo = 0;

            while (!reader.EndOfStream)
            {
                var line = await reader.ReadLineAsync();
                lineNo++;
                var splitter = line.Split(',');
                if (lineNo == 1)
                {
                    var rollData = splitter.FirstOrDefault(x => x.ToLower() == RollColumn);
                    if (!string.IsNullOrEmpty(rollData))
                    {
                        rollCol = splitter.ToList().IndexOf(rollData);
                    }

                    var pointData = splitter.FirstOrDefault(x => x.ToLower() == PointColumn);
                    if (!string.IsNullOrEmpty(pointData))
                    {
                        gradePointCol = splitter.ToList().IndexOf(pointData);
                    }

                    var gradeData = splitter.FirstOrDefault(x => x.ToLower() == GradeColumn);
                    if (!string.IsNullOrEmpty(gradeData))
                    {
                        gradeCol = splitter.ToList().IndexOf(gradeData);
                    }
                    continue;
                }


                var roll = splitter[rollCol];
                decimal.TryParse(splitter[gradePointCol], out decimal gradePoint);
                var gradeName = splitter[gradeCol];

                var courseStudent = await _db.StudentCourses
                                             .FirstOrDefaultAsync(x => x.Student.UserName == roll &&
                                                                       x.Course.Id == course.Id);
                if (courseStudent == null)
                {
                    var student = await _db.DBUsers
                                           .FirstOrDefaultAsync(x => x.UserName == roll);
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
            _notificationService.SendNotification(batch.Name, course.CourseId, "Result Has been published");
            return new ActionResponse(true, "Result Uploaded Successfully");
        }

        public async Task<List<Course>> GetSemesterCoursesAsync(int semesterId)
        {
            return await _db.Courses
                            .Where(x => x.Semester.Id == semesterId)
                            .ToListAsync();
        }

        public async Task<List<Course>> SearchCourse(string search)
        {
            var courseList = await _db.Courses
                                      .Include(x => x.Semester)
                                      .ThenInclude(m => m.Batch)
                                      .Where(n => EF.Functions.Like(n.CourseId, $"%{search}%") ||
                                                EF.Functions.Like(n.CourseName, $"%{search}%"))
                                      .OrderByDescending(x => x.Semester.EndsOn)
                                      .ToListAsync();
            return courseList;
        }



        public async Task<ActionResponse> DeleteLesson(string userId, int lessonId)
        {
            var user = await _db.DBUsers
                                .Include(x => x.Batch)
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

        public async Task<List<SemesterData>> GetResult(string userId)
        {
            var allSemesters = new List<SemesterData>();
            var res = await _db.StudentCourses.Include(x => x.Course)
                                              .ThenInclude(x => x.Semester)
                                              .Where(x => x.Student.Id == userId)
                                              .ToListAsync();
            var grouped = res.GroupBy(x => x.Course.Semester.Id).ToList();
            foreach (var courseData in grouped)
            {
                var semesterData = courseData.FirstOrDefault().Course.Semester;
                var data = new SemesterData(semesterData, courseData.ToList());
                if (data.SemesterGPA > 0)
                {
                    allSemesters.Add(data);
                }
            }
            return allSemesters.OrderBy(x => x.StartsOn).ToList();
        }

        #endregion

        #region Material
        public async Task<ActionResponse> AddMaterial(int courseId, List<DBFile> dbFiles)
        {
            var course = await _db.Courses.Include(x => x.CourseMaterials)
                                          .FirstOrDefaultAsync(x => x.Id == courseId);
            if (course == null)
            {
                return new ActionResponse(false, "Invalid Course Information");
            }
            else
            {
                dbFiles.ForEach(x => course.CourseMaterials.Add(x));
                await _db.SaveChangesAsync();
                return new ActionResponse(true, "Course Material Uploaded Successfully");
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

        public async Task<ActionResponse> AddUpdateLesson(int courseId, Lesson lesson)
        {
            if (lesson.Id == 0)
            {
                var course = await _db.Courses.FirstOrDefaultAsync(x => x.Id == courseId);
                if (course == null)
                {
                    return new ActionResponse(false, "Invalid Course Information");
                }
                lesson.Course = course;
                _db.Entry(lesson).State = EntityState.Added;
            }
            else
            {
                _db.Entry(lesson).State = EntityState.Modified;
            }
            await _db.SaveChangesAsync();
            return new ActionResponse(true);
        }

        #endregion

    }
}
