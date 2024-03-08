global using Microsoft.EntityFrameworkCore;
using AviasalesApi.Models;

namespace AviasalesApi
{
    public class DataContext(DbContextOptions options, IConfiguration config) : DbContext(options)
    {
        private readonly IConfiguration _config = config;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(_config.GetConnectionString("PostgresConnection"));
        }

        public DbSet<User> Users => Set<User>();
    }
}
