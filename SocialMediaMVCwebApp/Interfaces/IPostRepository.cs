using SocialMediaMVCwebApp.ViewModels;

namespace SocialMediaMVCwebApp.Interfaces
{
    public interface IPostRepository
    {
        IEnumerable<PostViewModel> GetAllPosts();
        public PostViewModel? GetById(int id);

    }
}
