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
        public DbSet<FriendRequests> FriendRequests { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder builder) =>
            builder.UseSqlServer(@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename=D:\internship\Database\InternshipDb.mdf;Integrated Security = True; Connect Timeout = 30");
    }
}
