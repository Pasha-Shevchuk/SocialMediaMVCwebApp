using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.FileProviders;

namespace SocialMediaMVCwebApp.ViewModels
{
    public class EditPostViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string PostText { get; set; }
        public IFormFile? Image { get; set; }
        public int PostCategoryId { get; set; }
        public List<SelectListItem>? PostCategories { get; set; }
        public string? Country { get; set; }
        public string? Location { get; set; }
        public string? Region { get; set; }
    }

}
