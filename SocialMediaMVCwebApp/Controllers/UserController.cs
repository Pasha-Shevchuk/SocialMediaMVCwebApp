using Microsoft.AspNetCore.Mvc;
using SocialMediaMVCwebApp.Interfaces;
using SocialMediaMVCwebApp.Models;
using SocialMediaMVCwebApp.ViewModels;

namespace SocialMediaMVCwebApp.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IPostRepository _postRepository; // Inject post repository

        public UserController(IUserRepository userRepository, IPostRepository postRepository)
        {
            _userRepository = userRepository;
            _postRepository = postRepository;
        }

        [HttpGet("users")]
        public async Task<IActionResult> Index()
        {
            var users = await _userRepository.GetAllUsers(); // Get users with Gender and Address included
            var userViewModels = users.Select(u => new UsersViewModel
            {
                Id = u.Id,
                UserName = u.UserName,
                GenderName = u.Gender?.NameOfGender, // Assuming Gender has a 'Name' property
                Country = u.Address?.Country,  // Assuming Address has 'Country'
                Location = u.Address?.Location, // Assuming Address has 'Location'
                Region = u.Address?.Region   // Assuming Address has 'Region'
            }).ToList();

            return View(userViewModels);
        }


        [HttpGet("users/details/{id}")]
        public async Task<IActionResult> Details(string id)
        {
            var user = await _userRepository.GetById(id);
            if (user == null)
            {
                return NotFound();
            }

            var userPosts = await _postRepository.GetPostsByUserId(id);

            var userDetailViewModel = new UserDetailsViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                GenderName = user.Gender?.NameOfGender,
                Country = user.Address?.Country,
                Location = user.Address?.Location,
                Region = user.Address?.Region,
                UserPosts = userPosts.Select(p => new PostViewModel
                {
                    Id = p.Id,
                    Title = p.Title,
                    PostText = p.PostText,
                    Image = p.Image,
                    PostCategoryId = p.PostCategoryId,
                    PostCategoryName = p.PostCategory?.NameOfPostCategory,
                    Country = p.Address?.Country,
                    Location = p.Address?.Location,
                    Region = p.Address?.Region,
                    AppUserId = p.AppUserId
                }).ToList()
            };

            return View(userDetailViewModel);
        }



    }

}
