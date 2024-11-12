namespace SocialMediaMVCwebApp.ViewModels
{
    public class UsersViewModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string? GenderName { get; set; }    // Display gender name
        public string Country { get; set; }       // Address country
        public string Location { get; set; }      // Address location (city/village)
        public string Region { get; set; }        // Address region (state/oblast)

    }
}
