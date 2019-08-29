using Microsoft.EntityFrameworkCore;
using TestingApp.API.Models;

namespace TestingApp.API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options): base(options) {}
        public DbSet<Value> Values { get; set; }
        public DbSet<User> Users { get; set; }
    }
}

// public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
// {
// }