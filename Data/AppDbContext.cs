using DatingApp.API.Configuration.Entities;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Data
{
    public class AppDbContext : IdentityDbContext<UserData>
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Value> Values { get; set; }
        public DbSet<UserData> UserDatas { get; set; }
        public DbSet<Photo> Photos { get; set; }

        protected override void OnModelCreating(ModelBuilder builder){
         base.OnModelCreating(builder);
         builder.ApplyConfiguration( new RoleConfiguration());
        }
        
        
    }
}