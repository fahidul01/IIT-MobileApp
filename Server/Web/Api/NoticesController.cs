using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoreEngine.APIHandlers;
using CoreEngine.Model.DBModel;
using Microsoft.AspNetCore.Mvc;
using Web.Infrastructure.Services;

namespace Web.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class NoticesController : Controller, INoticeHandler
    {
        private readonly NoticeService _noticeService;

        public NoticesController(NoticeService noticeService)
        {
            _noticeService = noticeService;
        }

        public async Task<bool> AddPost(Notice post)
        {
            var user = HttpContext.User.Identity.Name;
            if (string.IsNullOrWhiteSpace(user)) return false;
            else return await _noticeService.AddUpdateNotice(post, user);
        }

        public Task<bool> DeletePost(Notice post)
        {
            throw new NotImplementedException();
        }

        public Task<List<Notice>> GetPosts(int page, PostType postType = PostType.All)
        {
            throw new NotImplementedException();
        }

        public Task<List<Notice>> GetUpcomingEvents(int v, PostType all)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdatePost(Notice post)
        {
            var user = HttpContext.User.Identity.Name;
            if (string.IsNullOrWhiteSpace(user)) return false;
            else return await _noticeService.AddUpdateNotice(post, user);
        }
    }
}