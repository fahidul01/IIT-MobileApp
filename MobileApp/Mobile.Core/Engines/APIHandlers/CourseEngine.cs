using CoreEngine.APIHandlers;
using CoreEngine.Model.DBModel;
using Mobile.Core.Worker;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mobile.Core.Engines.APIHandlers
{
    class CourseEngine : BaseEngine, ICourseHandler
    {
        public CourseEngine(HttpWorker httpWorker) : base(httpWorker)
        {
        }

        public Task<bool> CreateCourse(Course course)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteCourse(Course course)
        {
            throw new NotImplementedException();
        }

        public Task<List<Course>> GetCourses()
        {
            throw new NotImplementedException();
        }

        public Task<List<Course>> GetCourses(int batchId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateCourse(Course course)
        {
            throw new NotImplementedException();
        }
    }
}
