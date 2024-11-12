using Microsoft.AspNetCore.Mvc.Rendering;

namespace SocialMediaMVCwebApp.ViewModels
{
    public class CreatePostViewModel
    {
        public string Title { get; set; }
        public string AppUserId { get; set; }
        public string PostText { get; set; }
        public IFormFile Image { get; set; }
        public int PostCategoryId { get; set; }
        public List<SelectListItem>? PostCategories { get; set; }

        // Address-related fields
        public string? Country { get; set; }
        public string? Location { get; set; }
        public string? Region { get; set; }
    }

}
