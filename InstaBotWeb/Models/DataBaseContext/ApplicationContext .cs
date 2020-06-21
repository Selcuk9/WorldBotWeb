using InstaBotWeb.Models.Telegram;
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
            //Database.EnsureCreated();
        }
        public DbSet<User> DbUsers { get; set; }
        public DbSet<TelegramBot> TelegramBots { get; set; }
        public DbSet<UserTelegram> UserTelegrams { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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


            modelBuilder.Entity<UserTelegram>()
                .HasKey(k => new {k.UserId, k.BotId});

            //Связь many-to-many
            modelBuilder.Entity<UserTelegram>()
                .HasOne(u => u.User)
                .WithMany(uT => uT.UserTelegrams)
                .HasForeignKey(uId => uId.UserId);

            modelBuilder.Entity<UserTelegram>()
                .HasOne(bId => bId.TelegramBot)
                .WithMany(uT => uT.UserTelegrams)
                .HasForeignKey(bId => bId.BotId);
            //
            modelBuilder.Entity<TelegramBot>()
                .HasKey(uId => uId.TokenId);

            modelBuilder.Entity<TelegramBot>()
                .Property(uId => uId.TokenId)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<TelegramBot>()
               .Property(t => t.Token)
               .IsRequired();

            modelBuilder.Entity<TelegramBot>()
                .Property(uB => uB.UsernameBots)
                .IsRequired();
        }
    }
}
