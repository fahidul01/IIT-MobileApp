using CoreEngine.APIHandlers;
using CoreEngine.Model.Common;
using CoreEngine.Model.DBModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Web.Infrastructure.Services;

namespace Web.Api
{
    [Authorize(Roles = AppConstants.Student,
    AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class NoticesController : Controller, INoticeHandler
    {
        private readonly NoticeService _noticeService;

        public NoticesController(NoticeService noticeService)
        {
            _noticeService = noticeService;
        }

        public async Task<ActionResponse> AddPost(Notice post)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var res = await _noticeService.AddUpdateNotice(post, userId);
            return res;
        }

        public async Task<ActionResponse> DeletePost(Notice post)
        {
            return new ActionResponse(await _noticeService.Delete(post.Id));
        }

        public async Task<List<Notice>> GetPosts(int page, PostType postType = PostType.All)
        {
            return await _noticeService.GetRecentNotice(page);
        }

        public async Task<List<Notice>> GetUpcomingEvents(int page, PostType all)
        {
            return await _noticeService.GetUpcomingEvents();
        }

        public async Task<ActionResponse> UpdatePost(Notice post)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var res = await _noticeService.AddUpdateNotice(post, userId);
            return res;
        }
    }
}