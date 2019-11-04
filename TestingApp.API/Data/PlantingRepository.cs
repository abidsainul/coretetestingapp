using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TestingApp.API.Helpers;
using TestingApp.API.Models;

namespace TestingApp.API.Data
{
    public class PlantingRepository : IPlantingRepository
    {
        private readonly DataContext _context;
        public PlantingRepository(DataContext context)
        {
            this._context = context;
        }

        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        public async Task<Like> GetLike(int userId, int recipientId)
        {
            return await _context.Likes.FirstOrDefaultAsync(u => u.LikerId == userId 
                && u.LikeeId == recipientId);
        }

        public async Task<PlantPhoto> GetMainPlantPhoto(int userId)
        {
            return await _context.PlantPhotos.Where(x=>x.UserId == userId).FirstOrDefaultAsync(p=>p.IsMain);
        }

        public async Task<User> GetUser(int id)
        {
            var user = await _context.Users.Include(p=> p.PlantPhotos).FirstOrDefaultAsync(x=>x.Id == id);
            return user;
        }

        public async Task<PagedList<User>> GetUsers(UserParams userParams)
        {
           //var users = await _context.Users.Include(p=>p.PlantPhotos).ToListAsync();
           var users = _context.Users.Include(p=>p.PlantPhotos).
            OrderByDescending(u => u.LastActive).AsQueryable();

            users = users.Where(u => u.Id != userParams.UserId);

            // Gender and date of birth filters

            users = users.Where(u => u.Gender == userParams.Gender);

            if(userParams.Likers) {
                var userLikers = await GetUserLikes(userParams.UserId, userParams.Likers);
                users = users.Where(u => userLikers.Contains(u.Id));
            }

            if(userParams.Likees) {
                 var userLikees = await GetUserLikes(userParams.UserId, userParams.Likers);
                users = users.Where(u => userLikees.Contains(u.Id));
            }

            if(userParams.MinAge !=18 || userParams.MaxAge!=99) {
                
                var minDob = DateTime.Today.AddYears(-userParams.MaxAge - 1);
                var maxDob = DateTime.Today.AddYears(-userParams.MinAge);
                users = users.Where(u => u.DateOfBirth >= minDob && u.DateOfBirth <= maxDob);
            }

            if(!String.IsNullOrEmpty(userParams.OrderBy)) {
                switch (userParams.OrderBy)
                {
                    case "created":
                        users = users.OrderByDescending(u => u.Created);
                        break;
                    default:
                        users = users.OrderByDescending(u => u.LastActive);
                        break;
                }
            }

           return await PagedList<User>.CreateAsync(users,userParams.PageNumber,userParams.PageSize);
        }

        private async Task<IEnumerable<int>> GetUserLikes (int id , bool likers) {
            var user = await _context.Users.
                    Include(x => x.Likers).
                    Include(x => x.Likees)
                    .FirstOrDefaultAsync(u => u.Id == id);

            if(likers) 
            {
                return user.Likers.Where(u => u.LikeeId == id).Select(i => i.LikerId);
            }
            else
            {
                return user.Likees.Where(u => u.LikerId == id).Select(i => i.LikeeId);
            }
        }

        public async Task<bool> SaveAll()
        {
           return await _context.SaveChangesAsync() > 0;
        }

        Task<PlantPhoto> IPlantingRepository.GetPlantPhoto(int id)
        {
            var photo = _context.PlantPhotos.FirstOrDefaultAsync(p => p.Id == id);
            return photo;
        }

    }
}