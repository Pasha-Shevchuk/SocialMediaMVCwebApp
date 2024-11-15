using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SocialMediaMVCwebApp.Data;
using SocialMediaMVCwebApp.Interfaces;
using SocialMediaMVCwebApp.Models;
using SocialMediaMVCwebApp.ViewModels;
using System.Security.Claims;

namespace SocialMediaMVCwebApp.Controllers
{
    public class PostController : Controller
    {
        private readonly IPostRepository _postRepository;
        private readonly IPhotoService _photoService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PostController(IPostRepository postRepository, IPhotoService photoService, IHttpContextAccessor httpContextAccessor)
        {
            _postRepository = postRepository;
            _photoService = photoService;
            _httpContextAccessor = httpContextAccessor;
        }

        // GET: 
        public async Task<IActionResult> Index()
        {
            IEnumerable<Post> posts = (await _postRepository.GetAllPosts()).Reverse();
            IEnumerable<PostViewModel> postViewModels = posts.Select(post => MapToViewModel(post));

            return View(postViewModels);
        }

        // GET: Detail
        public async Task<IActionResult> Detail(int id)
        {
            Post post = await _postRepository.GetById(id);
            if (post == null) return NotFound();

            var comments = await _postRepository.GetCommentsByPostId(id);

            PostViewModel postViewModel = new PostViewModel
            {
                Id = post.Id,
                Title = post.Title,
                PostText = post.PostText,
                Image = post.Image,
                PostCategoryName = post.PostCategory.NameOfPostCategory,
                Country = post.Address?.Country,
                Location = post.Address?.Location,
                Region = post.Address?.Region,
                AppUserId = post.AppUserId,
                Comments = comments.Select(c => new CommentViewModel
                {
                    Id = c.Id,
                    Text = c.Text,
                    UserName = c.AppUser.UserName,
                    CreatedAt = c.CreatedAt
                }).ToList()
            };

            return View(postViewModel);
        }

        // GET: Create
        public async Task<IActionResult> Create()
        {
            // Fetch post categories from the repository
            IEnumerable<PostCategory> categories = await _postRepository.GetAllPostCategories();

            // Set the AppUserId (assuming you have access to the user ID from the current user context)
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            CreatePostViewModel model = new CreatePostViewModel
            {
                AppUserId = userId,  // Set the AppUserId here
                PostCategories = categories.Select(pc => new SelectListItem
                {
                    Value = pc.Id.ToString(),
                    Text = pc.NameOfPostCategory
                }).ToList()
            };

            return View(model);
        }
        
        // POST: Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreatePostViewModel model)
        {
            if (ModelState.IsValid)
            {
                ImageUploadResult result = await _photoService.AddPhotoAsync(model.Image, 400, 600);

                // Create a new Post
                Post post = new Post
                {
                    Title = model.Title,
                    PostText = model.PostText,
                    Image = result.Url.ToString(),  // You may want to add file upload handling here
                    PostCategoryId = model.PostCategoryId,
                    AppUserId = model.AppUserId,
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
            IEnumerable<PostCategory> categories = await _postRepository.GetAllPostCategories();
            model.PostCategories = categories.Select(pc => new SelectListItem
            {
                Value = pc.Id.ToString(),
                Text = pc.NameOfPostCategory
            }).ToList();

            return View(model);
        }

        // GET: Edit
        public async Task<IActionResult> Edit(int id)
        {
            
            // Fetch the post by its ID
            Post post = await _postRepository.GetById(id);
            if (post == null)
            {
                return NotFound();
            }

            // Check if the current user is the owner of the post
            var currentUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (post.AppUserId != currentUserId) // Prevent access to other users' posts
            {
                return Forbid(); // Or redirect to an error page
            }

            // Fetch post categories to populate the dropdown list
            IEnumerable<PostCategory> categories = await _postRepository.GetAllPostCategories();

            // Map the post data to the EditPostViewModel
            EditPostViewModel model = new EditPostViewModel
            {
                Id = post.Id,
                Title = post.Title,
                PostText = post.PostText,
                PostCategoryId = post.PostCategoryId,
                PostCategories = categories.Select(pc => new SelectListItem
                {
                    Value = pc.Id.ToString(),
                    Text = pc.NameOfPostCategory
                }).ToList(),
                Country = post.Address?.Country,
                Location = post.Address?.Location,
                Region = post.Address?.Region
            };

         

            return View(model);
        }

        // POST: Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditPostViewModel model)
        {
            if (ModelState.IsValid)
            {
                string imageUrl = model.Image != null
                    ? (await _photoService.AddPhotoAsync(model.Image, 400, 600)).Url.ToString()
                    : null;  // Or set a default image URL if no image is uploaded.

                // Fetch the existing post to update
                Post post = await _postRepository.GetById(model.Id);
                if (post == null)
                {
                    return NotFound();
                }

                // Update the post properties
                post.Title = model.Title;
                post.PostText = model.PostText;
                if (!string.IsNullOrEmpty(imageUrl))
                {
                    post.Image = imageUrl;  // Update the image URL if a new image was uploaded
                }
                post.PostCategoryId = model.PostCategoryId;
                post.Address.Country = model.Country;
                post.Address.Location = model.Location;
                post.Address.Region = model.Region;

                // Update the post in the repository
                _postRepository.Update(post);
                _postRepository.Save();

                return RedirectToAction("Index");
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

        // GET: /Post/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var post = await _postRepository.GetById(id);
            if (post == null) return NotFound();

            // Check if the current user is the owner of the post
            var currentUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (post.AppUserId != currentUserId) // Prevent access to other users' posts
            {
                return Forbid(); // Or redirect to an error page
            }


            var viewModel = new DeletePostViewModel
            {
                Id = post.Id,
                Title = post.Title,
                ImageUrl = post.Image
            };

            return View(viewModel);
        }

        // POST: /Post/Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(DeletePostViewModel viewModel)
        {
            var post = await _postRepository.GetById(viewModel.Id);
            if (post == null) return NotFound();

           
            if (!string.IsNullOrEmpty(post.Image))
            {
                await _photoService.DeletePhotoAsync(post.Image); // Delete the photo from Cloudinary
            }

            _postRepository.Delete(post); // Remove the post from the database
            return RedirectToAction("Index"); // Redirect to a page like Index after deletion
        }

        [HttpPost]
        public async Task<IActionResult> AddComment(int postId, string commentText)
        {
            // Get the current user's ID
            string userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            // Create a new Comment instance with the provided text and post ID
            Comment newComment = new Comment
            {
                Text = commentText,
                PostId = postId,
                AppUserId = userId,
                CreatedAt = DateTime.UtcNow
            };

            // Add the comment to the database through the repository
            var success = await _postRepository.AddComment(newComment);
            if (!success)
            {
                // Handle the error if saving failed
                ModelState.AddModelError(string.Empty, "Failed to add the comment.");
                return RedirectToAction("Detail", new { id = postId });
            }

            // Redirect back to the Detail page to show the updated comments
            return RedirectToAction("Detail", new { id = postId });
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
