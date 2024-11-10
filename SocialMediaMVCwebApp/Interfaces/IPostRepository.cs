using SocialMediaMVCwebApp.Models;
using SocialMediaMVCwebApp.ViewModels;

namespace SocialMediaMVCwebApp.Interfaces
{
    public interface IPostRepository
    {
        Task<IEnumerable<Post>> GetAllPosts();
        Task<Post> GetById(int id);

        // crud
        bool Add(Post post);
        bool Update(Post post);
        bool Delete(Post post);
        bool Save();


    }
}
