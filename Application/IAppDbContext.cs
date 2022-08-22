using Domain;
using Microsoft.EntityFrameworkCore;

namespace Application
{
    public interface IAppDbContext
    {
        DbSet<Friends> Friends { get; set; }
        DbSet<FriendRequests> FriendRequests { get; set; }
        DbSet<Message> Messages { get; set; }
        DbSet<User> Users { get; set; }

        Task<int> SaveChanges();
    }
}