using System.ComponentModel.DataAnnotations;

namespace SocialMediaMVCwebApp.Models
{
    public class Gender
    {
        [Key]
        public int Id { get; set; }
        public required string NameOfGender { get; set; }
    }
}