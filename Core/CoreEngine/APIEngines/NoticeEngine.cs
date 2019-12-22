﻿using CoreEngine.APIHandlers;
using CoreEngine.Engine;
using CoreEngine.Model.Common;
using CoreEngine.Model.DBModel;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace CoreEngine.APIEngines
{
    class NoticeEngine : BaseApiEngine, INoticeHandler
    {
        private const string Controller = "Notices";
        public NoticeEngine(HttpWorker httpWorker) : base(httpWorker, Controller)
        {
        }

        public Task<ActionResponse> AddPost(Notice notice)
        {
            return SendRequest<ActionResponse>(HttpMethod.Post, notice);
        }

        public Task<ActionResponse> DeletePost(Notice notice)
        {
            return SendRequest<ActionResponse>(HttpMethod.Post, notice);
        }

        public Task<Notice> GetNotice(int noticeId)
        {
            return SendRequest<Notice>(HttpMethod.Get, new { noticeId });
        }

        public Task<List<Notice>> GetPosts(int page, PostType postType = PostType.All)
        {
            return SendRequest<List<Notice>>(HttpMethod.Post, new { page, postType });
        }

        public Task<List<Notice>> GetUpcomingEvents(int page, PostType postType)
        {
            return SendRequest<List<Notice>>(HttpMethod.Post, new { page, postType });
        }

        public Task<ActionResponse> UpdatePost(Notice post)
        {
            return SendRequest<ActionResponse>(HttpMethod.Post, post);
        }
    }
}