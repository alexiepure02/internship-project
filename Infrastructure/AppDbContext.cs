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
        //public DbSet<Friends> Friends { get; set; }
        //public DbSet<FriendRequests> FriendRequests { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder builder) =>
            builder.UseSqlServer(@"Data Source=IEPURE\SQLEXPRESS;DataBase=InternshipProjectDb;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<User>().HasMany(u => u.MainUserFriends).WithMany();

            //modelBuilder.Entity<User>().Property(u => u.Username).IsRequired();

            modelBuilder.Entity<Friends>()
            .HasKey(f => new { f.IDUser, f.IDFriend });

            modelBuilder.Entity<Friends>()
                .HasOne(f => f.User)
                .WithMany(mu => mu.MainUserFriends)
                .HasForeignKey(f => f.IDUser).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Friends>()
                .HasOne(f => f.Friend)
                .WithMany(mu => mu.Friends1)
                .HasForeignKey(f => f.IDFriend);

            modelBuilder.Entity<User>().HasMany(u => u.Messages).WithOne(u => u.Sender).HasForeignKey(u => u.IDSender).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Message>().HasOne(u => u.Receiver);

            // same thing for friend requests

            // add validatione

            // update code when using Friends - .ID not the same as .IDSender

            // modelBuilder.Entity<User>().HasMany(m => m.Followers).WithMany(p => p.Following).Map(w => w.ToTable("User_Follow").MapLeftKey("UserId").MapRightKey("FollowerID"));
        }
    }
}
