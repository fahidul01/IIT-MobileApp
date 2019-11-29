using System.Collections.Generic;
using System.Threading.Tasks;
using CoreEngine.APIHandlers;
using CoreEngine.Model.Common;
using CoreEngine.Model.DBModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Infrastructure.Services;

namespace Web.Api
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CoursesController : ControllerBase, ICourseHandler
    {
        private readonly CourseService _courseService;
        private readonly UserService _userservice;

        public CoursesController(CourseService courseService, UserService userService)
        {
            _courseService = courseService;
            _userservice = userService;
        }

        public Task<ActionResponse> AddMaterial(int courseId, DBFile dbFile)
        {
            throw new System.NotImplementedException();
        }

        public async Task<ActionResponse> CreateCourse(Course course, int semesterId)
        {
            var batch = await _userservice.GetBatch(HttpContext.User.Identity.Name);
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

        public async Task<ActionResponse> DeleteCourse(Course course)
        {
            var batch = await _userservice.GetBatch(HttpContext.User.Identity.Name);
            if (batch == null)
            {
                return new ActionResponse(false, "Invalid User");
            }
            else
                return new ActionResponse(await _courseService.Delete(course.Id, batch.Id));
        }

        public Task<ActionResponse> DeleteCouseMaterial(DBFile obj)
        {
            throw new System.NotImplementedException();
        }

        public Task<ActionResponse> DeleteLesson(int lessonId)
        {
            throw new System.NotImplementedException();
        }

        public async Task<Course> GetCourse(int courseId)
        {
            return await _courseService.GetCourseAsync(courseId);
        }

        public async Task<List<Course>> GetCourses()
        {
            var batch = _userservice.GetBatch(HttpContext.User.Identity.Name);
            return await _courseService.GetCoursesAsync(batch.Id);
        }

        public async Task<List<Course>> GetCourses(int batchId)
        {
            return await _courseService.GetCoursesAsync(batchId);
        }

        public Task<ActionResponse> Update(Lesson lesson)
        {
            throw new System.NotImplementedException();
        }

        public Task<ActionResponse> UpdateCourse(Course course)
        {
            throw new System.NotImplementedException();
        }
    }
}
