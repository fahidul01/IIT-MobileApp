using CoreEngine.Model.DBModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CoreEngine.APIHandlers
{
    public interface ICourseHandler
    {
        Task<bool> CreateCourse(Course course);
        Task<List<Course>> GetCourses();
        Task<List<Course>> GetCourses(int batchId);
        Task<bool> UpdateCourse(Course course);
        Task<bool> DeleteCourse(Course course);
    }
}
