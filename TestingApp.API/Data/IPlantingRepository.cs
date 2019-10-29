using System.Collections.Generic;
using System.Threading.Tasks;
using TestingApp.API.Models;

namespace TestingApp.API.Data
{
    public interface IPlantingRepository
    {
         void Add<T>(T entity) where T:class;
         void Delete<T>(T entity) where T:class;
         Task<bool> SaveAll();
         Task<IEnumerable<User>> GetUsers();
         Task<User> GetUser(int id);
         Task<PlantPhoto> GetPlantPhoto(int id);
        Task<PlantPhoto> GetMainPlantPhoto(int userId);

    }
}