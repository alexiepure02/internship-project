using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public interface IUserRepository
    {
        bool ValidateNewUser(User user);
        Task CreateUserAsync(User user);
        Task CreateFriendRequestAsync(FriendRequests friendRequest);
        Task UpdateFriendRequestAsync(FriendRequests friendRequest, bool accepted);
        Task DeleteFriendAsync(Friends friend);
        Task<List<Friends>> GetAllFriendsOfUserAsync(int idUser);
        Task<List<FriendRequests>> GetAllFriendRequestsOfUserAsync(int idUser);
        Task<List<User>> GetAllUsersAsync();
        Task<Friends> GetFriendOfUserAsync(int idUser, int idFriend);
        Task<FriendRequests> GetFriendRequestOfUserAsync(int idUser, int idRequester);
        Task<User> GetUserByAccountAsync(string username, string password);
        Task<User> GetUserByIdAsync(int idUser);
    }
}
