using HakatonServer.Models;
using Microsoft.EntityFrameworkCore;

namespace HakatonServer.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

            Database.EnsureCreated();
        }
        public DbSet<News> News { get; set; }
        public DbSet<User> Users { get; set; }
    }
}

//  Message='AddDbContext' was called with configuration,
//  but the context type 'AppDbContext' only declares a parameterless constructor.
//  This means that the configuration passed to 'AddDbContext' will never be used.
//  If configuration is passed to 'AddDbContext', then 'AppDbContext'
//  should declare a constructor that accepts a DbContextOptions<AppDbContext> and must pass it to the base constructor for DbContext.
