using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialMediaMVCwebApp.Models
{
    public class AppUser : IdentityUser
    {

        [ForeignKey("AddressId")]
        public int? AddressId{ get; set; }
        public Address? Address{ get; set; }

        [ForeignKey("Gender")]
        public int? GenderId { get; set; }
        public Gender? Gender { get; set; }

        public ICollection<Post> Posts { get; set; }

    }
}
