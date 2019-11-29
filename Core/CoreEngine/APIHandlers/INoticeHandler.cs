using CoreEngine.Model.Common;
using CoreEngine.Model.DBModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreEngine.APIHandlers
{
    public interface INoticeHandler
    {
        Task<List<Notice>> GetPosts(int page, PostType postType = PostType.All);
        Task<ActionResponse> AddPost(Notice notice);
        Task<ActionResponse> UpdatePost(Notice notice);
        Task<ActionResponse> DeletePost(Notice notice);
        Task<List<Notice>> GetUpcomingEvents(int page, PostType all);
    }
}
