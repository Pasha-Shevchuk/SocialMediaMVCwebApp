﻿using Microsoft.EntityFrameworkCore;
using SocialMediaMVCwebApp.Data;
using SocialMediaMVCwebApp.Interfaces;
using SocialMediaMVCwebApp.Models;
using SocialMediaMVCwebApp.ViewModels;

namespace SocialMediaMVCwebApp.Repository
{
    public class PostRepository : IPostRepository
    {
        private readonly AppDbContext _context;

        public PostRepository(AppDbContext context)
        {
            _context = context;
        }

        public bool Add(Post post)
        {
            _context.Add(post);
            return Save();
        }

        public bool Delete(Post post)
        {
            _context.Remove(post);
            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0? true: false;
        }

        public bool Update(Post post)
        {
            _context.Update(post);
            return Save();
        }

        public async Task<IEnumerable<Post>> GetAllPosts()
        {
            return await _context.Posts
                .Include(p => p.PostCategory)
                .Include(p => p.Address)
                .ToListAsync();
        }

        public async Task<Post> GetById(int id)
        {
            var post =  await _context.Posts
                .Include(p => p.PostCategory)
                .Include(p => p.Address)
                .FirstOrDefaultAsync(p => p.Id == id);
            return post;
        }


        public async Task<IEnumerable<Post>> GetPostsByUserId(string userId)
        {
            return await _context.Posts
                .Where(p => p.AppUserId == userId)
                .Include(p => p.PostCategory)
                .Include(p => p.Address)
                .ToListAsync();
        }

        public async Task<IEnumerable<PostCategory>> GetAllPostCategories()
        {
            return await _context.PostCategories.ToListAsync();
        }


        public async Task<bool> AddComment(Comment comment)
        {
            _context.Comments.Add(comment);
            return Save();
        }

        public async Task<IEnumerable<Comment>> GetCommentsByPostId(int postId)
        {
            return await _context.Comments
                .Where(c => c.PostId == postId)
                .Include(c => c.AppUser)
                .ToListAsync();
        }

    }

}
