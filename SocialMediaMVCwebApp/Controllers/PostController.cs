using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        private readonly IPhotoService _photoService;

        public PostController(IPostRepository postRepository, IPhotoService photoService)
        {
            _postRepository = postRepository;
            _photoService = photoService;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Post> posts = await _postRepository.GetAllPosts();
            IEnumerable<PostViewModel> postViewModels = posts.Select(post => MapToViewModel(post));

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


        public async Task<IActionResult> Create()
        {
            // Fetch post categories from the repository
            IEnumerable<PostCategory> categories = await _postRepository.GetAllPostCategories();

            var model = new CreatePostViewModel
            {
                PostCategories = categories.Select(pc => new SelectListItem
                {
                    Value = pc.Id.ToString(),
                    Text = pc.NameOfPostCategory
                }).ToList()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreatePostViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _photoService.AddPhotoAsync(model.Image, 400, 600);

                // Create a new Post
                var post = new Post
                {
                    Title = model.Title,
                    PostText = model.PostText,
                    Image = result.Url.ToString(),  // You may want to add file upload handling here
                    PostCategoryId = model.PostCategoryId,
                    Address = new Address
                    {
                        Country = model.Country,
                        Location = model.Location,
                        Region = model.Region
                    }
                };

                // Add the post to the repository
                _postRepository.Add(post);

                return RedirectToAction("Index"); // Redirect to the list of posts after creating
            }

            // If validation failed, fetch the post categories again
            var categories = await _postRepository.GetAllPostCategories();
            model.PostCategories = categories.Select(pc => new SelectListItem
            {
                Value = pc.Id.ToString(),
                Text = pc.NameOfPostCategory
            }).ToList();

            return View(model);
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
