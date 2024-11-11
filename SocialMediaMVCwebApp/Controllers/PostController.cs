﻿using Microsoft.AspNetCore.Mvc;
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
            if (post == null)
            {
                return NotFound();
            }

            PostViewModel postViewModel = MapToViewModel(post);
            return View(postViewModel);
        }


        // GET: Create
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
        // POST: Create
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

        // GET: Edit
        public async Task<IActionResult> Edit(int id)
        {
            // Fetch the post by its ID
            Post post = await _postRepository.GetById(id);
            if (post == null)
            {
                return NotFound();
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
