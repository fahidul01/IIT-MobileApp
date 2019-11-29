using CoreEngine.Model.Common;
using CoreEngine.Model.DBModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreEngine.APIHandlers
{
    public interface ICourseHandler
    {
        Task<ActionResponse> CreateCourse(Course course, int semesterId);
        Task<List<Course>> GetCourses();
        Task<List<Course>> GetCourses(int batchId);
        Task<ActionResponse> UpdateCourse(Course course);
        Task<ActionResponse> DeleteCourse(Course course);
        Task<ActionResponse> Update(Lesson lesson);
        Task<ActionResponse> DeleteCouseMaterial(DBFile obj);
        Task<ActionResponse> AddMaterial(int courseId, DBFile dbFile);
        Task<Course> GetCourse(int courseId);
        Task<ActionResponse> DeleteLesson(int lessonId);
    }
}
