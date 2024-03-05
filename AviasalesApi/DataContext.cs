global using Microsoft.EntityFrameworkCore;
using AviasalesApi.Models;
using System.CodeDom;

namespace AviasalesApi
{
    public class DataContext(DbContextOptions options, IConfiguration config) : DbContext(options)
    {
        private readonly IConfiguration _config = config;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_config.GetConnectionString("DefaultConnection"));
        }

        public DbSet<User> Users => Set<User>();
    }
}
