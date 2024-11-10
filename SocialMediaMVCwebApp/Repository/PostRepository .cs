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

        public IEnumerable<PostViewModel> GetAllPosts()
        {
            IEnumerable<PostViewModel> posts = _context.Posts
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
                }).ToList();
            return posts;
        }

        public PostViewModel GetById(int id)
        {
            PostViewModel? post = _context.Posts
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
             .FirstOrDefault();

            return post;
        }




    }
}
