using SocialMediaMVCwebApp.ViewModels;

namespace SocialMediaMVCwebApp.Interfaces
{
    public interface IPostRepository
    {
        Task<IEnumerable<PostViewModel>> GetAllPosts();
        Task<PostViewModel> GetById(int id);

    }
}
