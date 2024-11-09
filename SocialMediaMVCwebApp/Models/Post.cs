using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialMediaMVCwebApp.Models
{
    public class Post
    {
        [Key]
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string PostText { get; set; }
        public string Image { get; set; }
        [ForeignKey("Address")]
        public int AddressId { get; set; }
        public Address Address { get; set; }
        [ForeignKey("PostCategory")]
        public int PostCategoryId { get; set; }
        public required PostCategory PostCategory { get; set; }
        [ForeignKey("AppUser")]
        public string? AppUserId { get; set; }
        public AppUser? AppUser { get; set; }
    }
}
