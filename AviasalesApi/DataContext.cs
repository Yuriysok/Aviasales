global using Microsoft.EntityFrameworkCore;
using AviasalesApi.Models.DB;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace AviasalesApi
{
    public class DataContext(DbContextOptions options, IConfiguration config) : IdentityDbContext(options)
    {
        private readonly IConfiguration _config = config;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(_config.GetConnectionString("PostgresConnection"));
        }

        //public DbSet<User> Users => Set<User>();
    }
}
