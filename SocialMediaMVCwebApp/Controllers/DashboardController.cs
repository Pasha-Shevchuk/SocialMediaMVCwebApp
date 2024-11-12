using Microsoft.AspNetCore.Mvc;
using SocialMediaMVCwebApp.Data;
using SocialMediaMVCwebApp.Interfaces;
using SocialMediaMVCwebApp.Models;
using SocialMediaMVCwebApp.ViewModels;

namespace SocialMediaMVCwebApp.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IDashboardRepository _dashboardRepository;

        public DashboardController(IDashboardRepository dashboardRepository)
        {
            _dashboardRepository = dashboardRepository;
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
    }
}
