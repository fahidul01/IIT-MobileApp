using CoreEngine.Model.Common;
using CoreEngine.Model.DBModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreEngine.APIHandlers
{
    public interface ICourseHandler
    {
        Task<ActionResponse> CreateCourse(Course course);
        Task<List<Course>> GetCourses();
        Task<List<Course>> GetCourses(int batchId);
        Task<ActionResponse> UpdateCourse(Course course);
        Task<ActionResponse> DeleteCourse(Course course);
        Task<Lesson> GetLessons(string courseId);
        Task<ActionResponse> Update(Lesson lesson);
    }
}
