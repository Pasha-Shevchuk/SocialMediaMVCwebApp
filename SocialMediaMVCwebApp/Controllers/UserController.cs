using Microsoft.AspNetCore.Mvc;
using SocialMediaMVCwebApp.Interfaces;
using SocialMediaMVCwebApp.Models;
using SocialMediaMVCwebApp.ViewModels;

namespace SocialMediaMVCwebApp.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
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
            var user = await _userRepository.GetById(id); // Get the user with Gender and Address included
            if (user == null)
            {
                return NotFound(); // Handle case if user is not found
            }

            var userDetailViewModel = new UserDetailsViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                GenderName = user.Gender?.NameOfGender, // Assuming Gender has 'NameOfGender' property
                Country = user.Address?.Country,        // Assuming Address has 'Country' property
                Location = user.Address?.Location,      // Assuming Address has 'Location' property
                Region = user.Address?.Region           // Assuming Address has 'Region' property
            };

            return View(userDetailViewModel);
        }



    }

}
