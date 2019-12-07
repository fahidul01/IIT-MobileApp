using CoreEngine.APIHandlers;
using CoreEngine.Model.Common;
using CoreEngine.Model.DBModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;
using Web.Infrastructure.Services;

namespace Web.Api
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CoursesController : ControllerBase, ICourseHandler
    {
        private readonly CourseService _courseService;
        private readonly UserService _userservice;
        private readonly UserManager<DBUser> _usermanager;

        public CoursesController(CourseService courseService, UserService userService, UserManager<DBUser> userManager)
        {
            _courseService = courseService;
            _userservice = userService;
            _usermanager = userManager;
        }
        #region Course
        public async Task<ActionResponse> CreateCourse(int semesterId, Course course, List<DBFile> dBFiles, List<IFormFile> formFiles = null)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var batch = await _userservice.GetBatch(userId);
            if (batch == null)
            {
                return new ActionResponse(false, "Invalid User");
            }
            else
            {
                var res = await _courseService.AddCourse(course, semesterId, batch.Id);
                if(res!= null && formFiles!= null && formFiles.Count > 0)
                {
                    return await AddMaterial(res.Id, null, formFiles);
                }
                else return new ActionResponse(res != null);
            }
        }

        

        public async Task<ActionResponse> UploadCourseResult(int courseId, DBFile dBFile, IFormFile formFile)
        {
            if(courseId != 0 && formFile != null)
            {
                var filePath = Path.GetTempFileName();

                try
                {
                    using (var stream = System.IO.File.Create(filePath))
                    {
                        await formFile.CopyToAsync(stream);
                    }

                    if (formFile.FileName.EndsWith("csv"))
                    {
                        var res = await _userService.UploadCSVStudents(filePath, id);
                        return PartialView("_Students", res);
                    }
                    else
                    {
                        Failed("Invalid File format");
                        return PartialView("_Students", new List<User>());
                    }
                }
                catch (Exception ex)
                {
                    Failed(ex.Message);
                    return PartialView("_Students", new List<User>());
                }
            }
            var res = await _courseService.UploadResult(courseId,formFile.)
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
                var res = await _courseService.UpdateCourse(course);
                return new ActionResponse(res != null);
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
            return await _courseService.GetCourseAsync(courseId);
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
           return 
        }

        public Task<ActionResponse> DeleteCouseMaterial(DBFile obj)
        {
            throw new System.NotImplementedException();
        }

        #endregion



        public async Task<List<Course>> GetCourses(int batchId)
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
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return await _courseService.DeleteLesson(userId, lesson);
        }

        
    }
}
