using CoreEngine.APIHandlers;
using CoreEngine.Model.Common;
using CoreEngine.Model.DBModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Student.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IIT.Server.Controllers
{
    [Authorize]
    public class CoursesController : ControllerBase, ICourseHandler
    {
        private readonly CourseService _courseService;
        private readonly UserService _userservice;
        private readonly UserManager<DBUser> _usermanager;
        private readonly FileService _fileService;

        public CoursesController(CourseService courseService,
            UserService userService, UserManager<DBUser> userManager,
            FileService fileService)
        {
            _courseService = courseService;
            _userservice = userService;
            _usermanager = userManager;
            _fileService = fileService;
        }
        #region Course
        public async Task<ActionResponse> CreateCourse(int semesterId, Course course, List<DBFile> dBFiles, List<IFormFile> formFiles = null)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var access = await _userservice.AuthorizeSemester(userId, semesterId);
            if (access)
            {
                var semester = await _courseService.GetSemesterAsync(semesterId);
                if (semester == null)
                {
                    return new ActionResponse(false, "Invalid User");
                }
                else
                {
                    var res = await _courseService.AddCourse(course, semesterId, semester.Batch.Id);
                    if (res != null && formFiles != null && formFiles.Count > 0)
                    {
                        return await AddMaterial(res.Id, null, formFiles);
                    }
                    else
                    {
                        return new ActionResponse(res != null);
                    }
                }
            }
            else
            {
                return new ActionResponse(false, "You may not have access to use this API");
            }
        }


        public async Task<ActionResponse> UploadCourseResult(int courseId, DBFile dBFile, IFormFile formFile)
        {
            if (courseId != 0 && formFile != null)
            {
                var filePath = Path.GetTempFileName();

                try
                {
                    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    var batch = await _userservice.GetBatch(userId);
                    using (var stream = System.IO.File.Create(filePath))
                    {
                        await formFile.CopyToAsync(stream);
                    }

                    if (formFile.FileName.EndsWith("csv"))
                    {
                        var uploadRes = await _courseService.UploadResult(courseId, batch.Id, filePath);
                        return uploadRes;
                    }
                    else
                    {
                        return new ActionResponse(false, "Invalid File Format");
                    }
                }
                catch (Exception ex)
                {
                    return new ActionResponse(false, ex.Message);
                }
            }
            else
            {
                return new ActionResponse(false, "Invalid Course or file");
            }
        }


        public async Task<ActionResponse> UpdateCourse(Course course)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var batch = await _userservice.GetBatch(userId);
            if (batch == null)
            {
                return new ActionResponse(false, "Invalid User");
            }
            else
            {
                var res = await _courseService.ModifyCourse(course);
                return new ActionResponse(res);
            }
        }

        public async Task<ActionResponse> DeleteCourse(Course course)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var batch = await _userservice.GetBatch(userId);
            if (batch == null)
            {
                return new ActionResponse(false, "Invalid User");
            }
            else
            {
                return new ActionResponse(await _courseService.Delete(course.Id, batch.Id));
            }
        }


        public async Task<Course> GetCourse(int courseId)
        {
            var res = await _courseService.GetCourseAsync(courseId);
            return res;
        }

        public async Task<List<Course>> GetCourses()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var batch = await _userservice.GetBatch(userId);
            if (batch == null)
            {
                return null;
            }
            else
            {
                return await _courseService.GetCoursesAsync(batch.Id);
            }

        }
        #endregion

        #region Course Material

        public async Task<ActionResponse> AddMaterial(int courseId, List<DBFile> dbFiles, List<IFormFile> formFiles = null)
        {
            if (courseId != 0 && formFiles != null && formFiles.Count > 0)
            {
                try
                {
                    dbFiles = await _fileService.UploadFiles(formFiles);
                    var res = await _courseService.AddMaterial(courseId, dbFiles);
                    return res;
                }
                catch (Exception ex)
                {
                    return new ActionResponse(false, ex.Message);
                }
            }
            return new ActionResponse(false, "Invalid File or Course Id");
        }

        public async Task<ActionResponse> DeleteCouseMaterial(DBFile obj)
        {
            var res = await _fileService.Delete(obj);
            return res;

        }

        #endregion



        public async Task<List<Course>> GetBatchCourses(int batchId)
        {
            return await _courseService.GetCoursesAsync(batchId);
        }

        public async Task<List<Semester>> GetCurrentSemester()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var batch = await _userservice.GetBatch(userId);
            return await _courseService.GetSemestersAsync(batch.Id);
        }

        public async Task<ActionResponse> DeleteLesson(int lessonId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return await _courseService.DeleteLesson(userId, lessonId);
        }

        public async Task<ActionResponse> Update(Lesson lesson)
        {
            var res = await _courseService.AddUpdateLesson(0, lesson);
            return res;
        }

        public async Task<List<SemesterData>> GetResult()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return null;
            }
            else
            {
                return await _courseService.GetResult(userId);
            }
        }

        [Authorize(Roles = AppConstants.Admin)]
        public async Task<List<SemesterData>> GetStudentResult(string userId)
        {
            return await _courseService.GetResult(userId);
        }

        public async Task<List<Course>> SearchCourse(string search)
        {
            return await _courseService.SearchCourse(search);
        }

        public Task<List<Course>> GetSemesterCourses(int semesterId)
        {
            return _courseService.GetSemesterCoursesAsync(semesterId);
        }
    }
}
