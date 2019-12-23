using CoreEngine.APIHandlers;
using CoreEngine.Model.Common;
using CoreEngine.Model.DBModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Student.Infrastructure.Services;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IIT.Server.Controllers
{
    [Authorize]
    public class LessonController : ControllerBase, ILessonHandler
    {
        private readonly LessonService _lessonService;
        private readonly UserService _userService;

        public LessonController(LessonService lessonService, UserService userService)
        {
            _lessonService = lessonService;
            _userService = userService;
        }
        public async Task<ActionResponse> AddLesson(int courseId, Lesson lesson)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var batch = await _userService.GetBatch(userId);

            var res = await _lessonService.AddLesson(courseId, batch.Id, lesson);
            return new ActionResponse(res != null);
        }

        public async Task<List<Lesson>> GetLessons()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return await _lessonService.GetLesson(userId);
        }

        public async Task<List<Lesson>> GetCourseLessons(int courseId)
        {
            return await _lessonService.GetCourseLesson(courseId);
        }

        public async Task<ActionResponse> UpdateLesson(Lesson lesson)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var batch = await _userService.GetBatch(userId);

            var res = await _lessonService.UpdateLesson(batch.Id, lesson);
            return new ActionResponse(res != null);
        }

        public Task<List<Lesson>> GetUpcomingLessons()
        {
            return _lessonService.UpcomingLessons();
        }
    }
}