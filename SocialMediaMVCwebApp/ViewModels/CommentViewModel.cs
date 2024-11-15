namespace SocialMediaMVCwebApp.ViewModels
{
    public class CommentViewModel
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string UserName { get; set; } // Display the commenter's username
        public DateTime CreatedAt { get; set; }
    }

}
