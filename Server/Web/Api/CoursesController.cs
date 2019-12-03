using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using CoreEngine.APIHandlers;
using CoreEngine.Model.Common;
using CoreEngine.Model.DBModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<ActionResponse> CreateCourse(Course course, int semesterId)
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
                return new ActionResponse(res != null);
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
                return new ActionResponse(await _courseService.Delete(course.Id, batch.Id));
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

        public async Task<ActionResponse> AddMaterial(int courseId, DBFile dbFile, IFormFile formFile)
        {
            return new ActionResponse(formFile!= null && formFile.Length>0);
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
