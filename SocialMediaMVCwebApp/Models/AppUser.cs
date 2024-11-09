using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialMediaMVCwebApp.Models
{
    public class AppUser : IdentityUser
    {
        public Address? Address{ get; set; }
        public ICollection<Post> Posts { get; set; }

        [ForeignKey("Gender")]
        public int? GenderId { get; set; }
        public Gender? Gender { get; set; }
    }
}
