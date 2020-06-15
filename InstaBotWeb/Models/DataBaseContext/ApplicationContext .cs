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
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }
        public DbSet<User> DbUser { get; set; }
        public DbSet<InstaUser> DbInstaUser { get; set; }
        public DbSet<UserInstaAndUser> InstaUserAndAccountUser { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserInstaAndUser>()
                 .HasKey(u => new { u.UserId, u.InstaUserId });


            modelBuilder.Entity<User>()
                .HasKey(uId => uId.Id);

            modelBuilder.Entity<User>()
                .Property(uId => uId.Id)
                .ValueGeneratedOnAdd();


            modelBuilder.Entity<User>()
                .Property(uId => uId.Password)
                .IsRequired();

            modelBuilder.Entity<User>()
                .Property(uId => uId.Email)
                .IsRequired();

            modelBuilder.Entity<User>()
                .Property(uId => uId.FirstName)
                .IsRequired();

            modelBuilder.Entity<User>()
                .Property(uId => uId.LastName)
                .IsRequired();

        }


        
    }
}
