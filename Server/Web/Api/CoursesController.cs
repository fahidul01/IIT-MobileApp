using System.Collections.Generic;
using System.Threading.Tasks;
using CoreEngine.APIHandlers;
using CoreEngine.Model.Common;
using CoreEngine.Model.DBModel;
using Microsoft.AspNetCore.Mvc;
using Web.Infrastructure.Services;

namespace Web.Api
{
    public class CoursesController : ControllerBase, ICourseHandler
    {
        private readonly CourseService _courseService;

        public CoursesController(CourseService courseService)
        {
            _courseService = courseService;
        }

        public Task<ActionResponse> AddMaterial(int courseId, DBFile dbFile)
        {
            throw new System.NotImplementedException();
        }

        public Task<ActionResponse> CreateCourse(Course course)
        {
           
        }

        public Task<ActionResponse> DeleteCourse(Course course)
        {
            throw new System.NotImplementedException();
        }

        public Task<ActionResponse> DeleteCouseMaterial(DBFile obj)
        {
            throw new System.NotImplementedException();
        }

        public Task<ActionResponse> DeleteLesson(int lessonId)
        {
            throw new System.NotImplementedException();
        }

        public Task<Course> GetCourse(int courseId)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<Course>> GetCourses()
        {
            throw new System.NotImplementedException();
        }

        public Task<List<Course>> GetCourses(int batchId)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<Semester>> GetCurrentSemester()
        {
            throw new System.NotImplementedException();
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
