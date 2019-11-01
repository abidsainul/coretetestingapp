using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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

        public async Task<PlantPhoto> GetMainPlantPhoto(int userId)
        {
            return await _context.PlantPhotos.Where(x=>x.UserId == userId).FirstOrDefaultAsync(p=>p.IsMain);
        }

        public async Task<User> GetUser(int id)
        {
            var user = await _context.Users.Include(p=> p.PlantPhotos).FirstOrDefaultAsync(x=>x.Id == id);
            return user;
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
           var users = await _context.Users.Include(p=>p.PlantPhotos).ToListAsync();
           return users;
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