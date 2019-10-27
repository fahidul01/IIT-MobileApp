using CoreEngine.Model.DBModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreEngine.APIHandlers
{
    public interface IPostHandler
    {
        Task<List<Notice>> GetPosts(int page, PostType postType = PostType.All);
        Task<bool> AddPost(Notice post);
        Task<bool> UpdatePost(Notice post);
        Task<bool> DeletePost(Notice post);
    }
}
