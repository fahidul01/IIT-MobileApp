using CoreEngine.Model.DBModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreEngine.APIHandlers
{
    public interface IPostHandler
    {
        Task<List<Post>> GetPosts(int page, PostType postType = PostType.All);
        Task<bool> AddPost(Post post);
        Task<bool> UpdatePost(Post post);
        Task<bool> DeletePost(Post post);
    }
}
