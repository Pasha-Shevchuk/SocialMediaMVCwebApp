namespace SocialMediaMVCwebApp.ViewModels
{
    public class PostViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string PostText { get; set; }
        public string Image { get; set; }

        // POST CATEGORY
        public int PostCategoryId { get; set; } // To store selected category ID
        public string PostCategoryName { get; set; } // For displaying category name
                                     
        
        // ADDRESS 
        public string Country { get; set; }
        public string Location { get; set; }
        public string Region { get; set; }
        // AppUserId (to check if the current user is the post owner)
        public string AppUserId { get; set; }
    }

}
