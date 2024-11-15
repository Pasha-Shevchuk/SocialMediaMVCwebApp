using SocialMediaMVCwebApp.Models;
using SocialMediaMVCwebApp.ViewModels;

namespace SocialMediaMVCwebApp.Interfaces
{
    public interface IPostRepository
    {
        Task<IEnumerable<Post>> GetAllPosts();
        Task<Post> GetById(int id);
        Task<IEnumerable<PostCategory>> GetAllPostCategories();
        // crud
        bool Add(Post post);
        bool Update(Post post);
        bool Delete(Post post);
        bool Save();
        Task<IEnumerable<Post>> GetPostsByUserId(string userId);
        Task<bool> AddComment(Comment comment);
        Task<IEnumerable<Comment>> GetCommentsByPostId(int postId);


    }
}
