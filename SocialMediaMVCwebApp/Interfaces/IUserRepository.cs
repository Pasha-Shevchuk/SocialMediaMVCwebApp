using Microsoft.EntityFrameworkCore;
using SocialMediaMVCwebApp.Models;

namespace SocialMediaMVCwebApp.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<AppUser>> GetAllUsers();
        Task<AppUser> GetById(string id);

        Task<IEnumerable<Gender>> GetAllGenders();
        bool Save();
        bool Delete(AppUser appUser);
        bool Update(AppUser appUser);
        bool Add(AppUser appUser);
    }
}
