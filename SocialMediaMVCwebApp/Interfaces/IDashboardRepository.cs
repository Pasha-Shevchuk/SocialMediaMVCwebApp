using SocialMediaMVCwebApp.Models;

namespace SocialMediaMVCwebApp.Interfaces
{
    public interface IDashboardRepository
    {
        Task<List<Post>> GetAllUserPosts();
    }
}
