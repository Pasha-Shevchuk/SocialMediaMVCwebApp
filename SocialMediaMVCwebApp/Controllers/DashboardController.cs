using Microsoft.AspNetCore.Mvc;
using SocialMediaMVCwebApp.Data;
using SocialMediaMVCwebApp.Interfaces;
using SocialMediaMVCwebApp.Models;
using SocialMediaMVCwebApp.Repository;
using SocialMediaMVCwebApp.ViewModels;
using System.Security.Claims;

namespace SocialMediaMVCwebApp.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IDashboardRepository _dashboardRepository;
        private readonly IUserRepository _userRepository;
        public DashboardController(IDashboardRepository dashboardRepository, IUserRepository userRepository)
        {
            _dashboardRepository = dashboardRepository;
            _userRepository = userRepository;
        }

        public async Task<IActionResult> Index()
        {
            List<Post> userPosts = await _dashboardRepository.GetAllUserPosts();
            userPosts.Reverse();

            DashboardViewModel model = new DashboardViewModel();
            {
                model.Posts = userPosts;
            }

            return View(model);
        }

        public async Task<IActionResult> UserDetails()
        {
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return NotFound();
            }

            var user = await _userRepository.GetById(userId);

            var userDetailViewModel = new UserDetailsViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                GenderName = user.Gender?.NameOfGender,
                Country = user.Address?.Country,
                Location = user.Address?.Location,
                Region = user.Address?.Region,
            };
            return View(userDetailViewModel);
        }

    }
}
