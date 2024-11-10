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

        public IActionResult Index()
        {
            IEnumerable<PostViewModel> posts = _postRepository.GetAllPosts();
            return View(posts);
        }

        public IActionResult Detail(int id)
        {
            PostViewModel post = _postRepository.GetById(id);
            if (post == null)
            {
                return NotFound();
            }
            return View(post);
        }

    }
}
