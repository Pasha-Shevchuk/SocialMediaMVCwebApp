using System.ComponentModel.DataAnnotations;

namespace SocialMediaMVCwebApp.Models
{
    public class PostCategory
    {
        [Key]
        public int Id { get; set; }
        public required string NameOfPostCategory { get; set; }
    }
}