using Microsoft.EntityFrameworkCore;
using TgVozderzhansBot.Core.Workers;
using TgVozderzhansBot.Models;

namespace TgVozderzhansBot.Database
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        
        public DbSet<AbsItem> AbsItems { get; set; }
        
        public DbSet<AbsGuardItem> AbsGuardItems { get; set; }
        
        public DbSet<ChangeAbsWaitItem> ChangeAbsWaitItems { get; set; }
        
        public DbSet<FriendItem> FriendItems { get; set; }
        
        public DbSet<InputFriendNickname> InputFriendNicknames { get; set; }
        
        public DbSet<InputMessageToFriend> InputMessageToFriends { get; set; }
        
        public DbSet<MailingItem> MailingItems { get; set; }
        
        public DbSet<PollItem> PollItems { get; set; }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5435;Database=tg_vozd;Username=postgres;Password=13781001");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AbsItem>().HasOne<User>();
            modelBuilder.Entity<User>().HasMany<AbsItem>();
            
            base.OnModelCreating(modelBuilder);
        }
    }
}