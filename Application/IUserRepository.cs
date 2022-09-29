using Domain;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public interface IUserRepository
    {
        Task<JwtSecurityToken> LoginAsync(string userName, string password);
        Task<string> RegisterAsync(string userName, string password, string displayName);
        Task<string> AddToRoleAsync(string userName, string roleName);
        Task CreateFriendRequestAsync(FriendRequests friendRequest);
        Task UpdateFriendRequestAsync(FriendRequests friendRequest, bool accepted);
        Task<bool> DeleteFriendAsync(Friends friend);
        Task<List<User>> GetAllFriendsOfUserAsync(int idUser);
        Task<List<User>> GetAllFriendRequestsOfUserAsync(int idUser);
        Task<List<User>> GetAllUsersAsync();
        Task<Friends> GetFriendOfUserAsync(int idUser, int idFriend);
        Task<FriendRequests> GetFriendRequestOfUserAsync(int idUser, int idRequester);
        Task<FriendRequests> GetFriendRequestByIdAsync(int id);
        Task<User> GetUserByIdAsync(int idUser);
    }
}
