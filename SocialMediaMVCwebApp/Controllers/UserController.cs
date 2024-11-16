using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SocialMediaMVCwebApp.Data;
using SocialMediaMVCwebApp.Interfaces;
using SocialMediaMVCwebApp.Models;
using SocialMediaMVCwebApp.ViewModels;

namespace SocialMediaMVCwebApp.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IPostRepository _postRepository; // Inject post repository
        private readonly AppDbContext _context;

        public UserController(IUserRepository userRepository, IPostRepository postRepository, AppDbContext context)
        {
            _userRepository = userRepository;
            _postRepository = postRepository;
            _context = context;
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

        [HttpGet("users/edit/{id}")]
        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userRepository.GetById(id);
            if (user == null)
            {
                return NotFound();
            }

            var genders = await _userRepository.GetAllGenders(); // Fetch the list of genders from the database
            var editUserViewModel = new UserEditViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                GenderId = user.GenderId,
                Country = user.Address?.Country,
                Location = user.Address?.Location,
                Region = user.Address?.Region,
                GenderOptions = new SelectList(genders, "Id", "NameOfGender", user.GenderId)
            };

            return View(editUserViewModel);
        }

        // make validation check
        [HttpPost("users/edit/{id}")]
        public async Task<IActionResult> Edit(UserEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.GenderOptions = new SelectList(await _context.Genders.ToListAsync(), "Id", "NameOfGender", model.GenderId);
                return View(model);
            }

            AppUser user = await _userRepository.GetById(model.Id);
            if (user == null)
            {
                return NotFound();
            }

            user.UserName = model.UserName;
            user.GenderId = model.GenderId;
            user.Address = new Address
            {
                Country = model.Country,
                Location = model.Location,
                Region = model.Region
            };

            _userRepository.Update(user);
            _userRepository.Save();

            return RedirectToAction("Details", new { id = user.Id });
        }




    }
}
