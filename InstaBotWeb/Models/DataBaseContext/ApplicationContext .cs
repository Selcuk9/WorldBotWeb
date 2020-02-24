using InstaBotWeb.ViewsModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InstaBotWeb.Models.DataBaseContext
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> DbUser { get; set; }
        public DbSet<InstaUser> DbInstaUser { get; set; }
        public DbSet<UserInstaAndUser> InstaUserAndAccountUser { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserInstaAndUser>()
                   .HasKey(u => new { u.UserId, u.InstaUserId });
        }

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) 
        {
           // Database.EnsureCreated(); // создаем базу данных при первом обращении
        }
    }
}
