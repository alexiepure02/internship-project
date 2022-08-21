using Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Friends> Friends { get; set; }
        public DbSet<FriendRequests> FriendsRequests { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder builder) =>
            builder.UseSqlServer(@"Data Source=IEPURE\SQLEXPRESS;DataBase=InternshipProjectDb;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // user id 

            modelBuilder.Entity<User>().Property(u => u.ID).ValueGeneratedNever();

            // many-to-many friends

            modelBuilder.Entity<Friends>()
                .HasOne(f => f.User)
                .WithMany(mu => mu.MainUserFriends)
                .HasForeignKey(f => f.IDFriend)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Friends>()
                .HasOne(f => f.Friend)
                .WithMany(mu => mu.Friends)
                .HasForeignKey(f => f.IDUser);

            // one-to-many messages

            modelBuilder.Entity<User>()
                .HasMany(u => u.Messages)
                .WithOne(u => u.Sender)
                .HasForeignKey(u => u.IDSender)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Message>()
                .HasOne(u => u.Receiver);

            // one-to-many friend requests
            
            modelBuilder.Entity<FriendRequests>()
                .HasOne(u => u.User)
                .WithMany(u => u.FriendRequests)
                .OnDelete(DeleteBehavior.Restrict);

            // user validation

            modelBuilder.Entity<User>().Property(x => x.ID).IsRequired();
            modelBuilder.Entity<User>().Property(x => x.Username).HasMaxLength(50);
            modelBuilder.Entity<User>().Property(x => x.Username).IsRequired();
            modelBuilder.Entity<User>().Property(x => x.Password).HasMaxLength(50);
            modelBuilder.Entity<User>().Property(x => x.Password).IsRequired();
            modelBuilder.Entity<User>().Property(x => x.DisplayName).HasMaxLength(50);
            modelBuilder.Entity<User>().Property(x => x.DisplayName).IsRequired();

            // message validation

            modelBuilder.Entity<Message>().Property(x => x.IDSender).IsRequired();
            modelBuilder.Entity<Message>().Property(x => x.IDReceiver).IsRequired();
            modelBuilder.Entity<Message>().Property(x => x.Text).HasMaxLength(256);

            base.OnModelCreating(modelBuilder);
        }
    }
}
