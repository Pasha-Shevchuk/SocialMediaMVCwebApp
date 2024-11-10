using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialMediaMVCwebApp.Data;
using SocialMediaMVCwebApp.Interfaces;
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
            IEnumerable<PostViewModel> posts = await _postRepository.GetAllPosts();
            return View(posts);
        }

        public async Task<IActionResult> Detail(int id)
        {
            PostViewModel post = await _postRepository.GetById(id);
            if (post == null)
            {
                return NotFound();
            }
            return View(post);
        }

    }
}
