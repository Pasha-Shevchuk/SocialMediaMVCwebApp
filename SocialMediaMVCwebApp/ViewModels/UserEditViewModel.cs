using Microsoft.AspNetCore.Mvc.Rendering;

namespace SocialMediaMVCwebApp.ViewModels
{
    public class UserEditViewModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public int? GenderId { get; set; } // To store selected gender ID
        public SelectList? GenderOptions { get; set; } // For displaying gender options
        public string? Country { get; set; }
        public string? Location { get; set; }
        public string? Region { get; set; }
    }

}
