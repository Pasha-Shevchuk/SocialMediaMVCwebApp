using Microsoft.EntityFrameworkCore;
using SocialMediaMVCwebApp.Data;
using SocialMediaMVCwebApp.Interfaces;
using SocialMediaMVCwebApp.Models;
using System.Runtime.InteropServices;
using System.Security.Claims;

namespace SocialMediaMVCwebApp.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public bool Add(AppUser appUser)
        {
            throw new NotImplementedException();
        }

        public bool Delete(AppUser appUser)
        {
            throw new NotImplementedException();
        }

        // Include Gender and Address when fetching all users
        public async Task<IEnumerable<AppUser>> GetAllUsers()
        {
            return await _context.Users
                .Include(u => u.Gender)    // Eager load the related Gender
                .Include(u => u.Address)   // Eager load the related Address
                .ToListAsync();
        }

        // Include Gender and Address when fetching a single user by ID
        public async Task<AppUser> GetById(string id)
        {
            return await _context.Users
                .Include(u => u.Gender)    // Eager load the related Gender
                .Include(u => u.Address)   // Eager load the related Address
                .FirstOrDefaultAsync(u => u.Id == id); // Use FirstOrDefaultAsync for retrieving by Id
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(AppUser appUser)
        {
            _context.Update(appUser);
            return Save();
        }
    }
}
