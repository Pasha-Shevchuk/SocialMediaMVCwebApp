using Microsoft.EntityFrameworkCore;
using SocialMediaMVCwebApp.Data;
using SocialMediaMVCwebApp.Interfaces;
using SocialMediaMVCwebApp.Models;
using System.Security.Claims;

namespace SocialMediaMVCwebApp.Repository
{
    public class DashboardRepository : IDashboardRepository
    {
        readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DashboardRepository(AppDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<Post>> GetAllUserPosts()
        {   // Get the current user's ID from the claims
            string? userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return new List<Post>(); // Return an empty list if user is not authenticated
            }

            // Filter posts by the AppUserId that matches the current user
            IQueryable<Post> userPosts = _context.Posts
                                                 .Where(p => p.AppUserId == userId)
                                                 .Include(p => p.PostCategory)
                                                 .Include(p => p.Address);

            return await userPosts.ToListAsync();
        }
    }
}
