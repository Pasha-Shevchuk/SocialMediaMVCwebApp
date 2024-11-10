using Microsoft.EntityFrameworkCore;
using SocialMediaMVCwebApp.Data;
using SocialMediaMVCwebApp.Interfaces;
using SocialMediaMVCwebApp.ViewModels;

namespace SocialMediaMVCwebApp.Repository
{
    public class PostRepository : IPostRepository
    {
        private readonly AppDbContext _context;

        public PostRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PostViewModel>> GetAllPosts()
        {
            var posts =  await _context.Posts
                .Include(p => p.PostCategory)
                .Include(p => p.Address)
                .Select(post => new PostViewModel
                {
                    Id = post.Id,
                    Title = post.Title,
                    PostText = post.PostText,
                    Image = post.Image,
                    PostCategoryId = post.PostCategoryId, // POST CATEGORY ID
                    PostCategoryName = post.PostCategory.NameOfPostCategory,
                    Country = post.Address.Country,
                    Location = post.Address.Location,
                    Region = post.Address.Region
                }).ToListAsync();
            return posts;
        }

        public async Task<PostViewModel> GetById(int id)
        {
            var post = await _context.Posts
             .Include(p => p.PostCategory)
             .Include(p => p.Address)
             .Where(p => p.Id == id)
             .Select(post => new PostViewModel
             {
                 Id = post.Id,
                 Title = post.Title,
                 PostText = post.PostText,
                 Image = post.Image,
                 PostCategoryId = post.PostCategoryId, // POST CATEGORY ID
                 PostCategoryName = post.PostCategory.NameOfPostCategory,
                 Country = post.Address.Country,
                 Location = post.Address.Location,
                 Region = post.Address.Region
             })
             .FirstOrDefaultAsync();

            return post;
        }

        




    }
}
