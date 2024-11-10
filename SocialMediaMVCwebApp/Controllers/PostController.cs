using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialMediaMVCwebApp.Data;
using SocialMediaMVCwebApp.Interfaces;
using SocialMediaMVCwebApp.Models;
using SocialMediaMVCwebApp.ViewModels;

namespace SocialMediaMVCwebApp.Controllers
{
    public class PostController : Controller
    {
        private readonly IPostRepository _postRepository;

        public PostController(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Post> posts = await _postRepository.GetAllPosts();
            var postViewModels = posts.Select(post => MapToViewModel(post));

            return View(postViewModels);
        }

        public async Task<IActionResult> Detail(int id)
        {
            Post post = await _postRepository.GetById(id);
            if (post == null)
            {
                return NotFound();
            }

            PostViewModel postViewModel = MapToViewModel(post);
            return View(postViewModel);
        }

        private PostViewModel MapToViewModel(Post post)
        {
            return new PostViewModel
            {
                Id = post.Id,
                Title = post.Title,
                PostText = post.PostText,
                Image = post.Image,
                PostCategoryId = post.PostCategoryId,
                PostCategoryName = post.PostCategory.NameOfPostCategory,
                Country = post.Address.Country,
                Location = post.Address.Location,
                Region = post.Address.Region
            };
        }
    }


}
