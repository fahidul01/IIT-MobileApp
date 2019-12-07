using CoreEngine.Model.Common;
using CoreEngine.Model.DBModel;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreEngine.APIHandlers
{
    public interface ICourseHandler
    {
        Task<ActionResponse> CreateCourse(int semesterId,Course course,List<DBFile> dBFiles, List<IFormFile> formFiles = null);
        Task<List<Course>> GetCourses();
        Task<List<Course>> GetCourses(int batchId);
        Task<ActionResponse> UpdateCourse(Course course);
        Task<ActionResponse> DeleteCourse(Course course);
        Task<ActionResponse> Update(Lesson lesson);
        Task<ActionResponse> DeleteCouseMaterial(DBFile dBFile);
        Task<ActionResponse> AddMaterial(int courseId, List<DBFile> dbFiles, List<IFormFile> formFiles = null);
        Task<Course> GetCourse(int courseId);
        Task<ActionResponse> DeleteLesson(int lessonId);
        Task<List<Semester>> GetCurrentSemester();
    }
}
