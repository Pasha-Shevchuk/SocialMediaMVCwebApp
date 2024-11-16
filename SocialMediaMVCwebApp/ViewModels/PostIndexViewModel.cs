using Microsoft.AspNetCore.Mvc.Rendering;

namespace SocialMediaMVCwebApp.ViewModels
{
    public class PostIndexViewModel
    {
        public IEnumerable<PostViewModel> Posts { get; set; }
        public List<SelectListItem> PostCategories { get; set; }
        public int? SelectedPostCategoryId { get; set; }

        public string SearchTitle { get; set; } 

    }

}
