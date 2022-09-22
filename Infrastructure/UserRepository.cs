using Application;
using Domain;
using Domain.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public bool ValidateNewUser(User user)
        {
            if (user.Username.Length < 5 && user.Username.Length > 50)
                return false;// "Length of username exceeds limit. (50)";
            if (user.Username.Length < 5 && user.Password.Length > 50)
                return false;// "Length of password exceeds limit. (50)";
            if (user.DisplayName.Length < 5 && user.DisplayName.Length > 50)
                return false;// "Length of display name exceeds limit. (50)";

            return true;
        }

        public async Task CreateUserAsync(User user)
        {
            if (ValidateNewUser(user))
                await _context.Users.AddAsync(user);
            else
                throw new InvalidUserException();
        }

        public async Task CreateFriendRequestAsync(FriendRequests friendRequest)
        {
            //var friendRequestDB = await _context.FriendRequests.Where(f => f.IDUser == friendRequest.IDUser && f.IDRequester == friendRequest.IDRequester).FirstOrDefaultAsync();

            var friendRequestDB = await _context.FriendRequests.SingleOrDefaultAsync(f => f.IDUser == friendRequest.IDUser && f.IDRequester == friendRequest.IDRequester);

            if (friendRequestDB == null)
                await _context.FriendRequests.AddAsync(friendRequest);
        }

        public async Task UpdateFriendRequestAsync(FriendRequests friendRequest, bool accepted)
        {
            // replace where with include

            //var friendRequestDB = await _context.FriendRequests.Where(f => f.IDUser == friendRequest.IDUser && f.IDRequester == friendRequest.IDRequester).FirstOrDefaultAsync();

            var friendRequestDB = await _context.FriendRequests.SingleOrDefaultAsync(f => f.IDUser == friendRequest.IDUser && f.IDRequester == friendRequest.IDRequester);

            if (friendRequestDB != null)
            {
                if (accepted)
                {
                    await _context.Friends.AddAsync(new Friends
                    {
                        IDUser = friendRequest.IDUser,
                        IDFriend = friendRequest.IDRequester
                    });

                    await _context.Friends.AddAsync(new Friends
                    {
                        IDUser = friendRequest.IDRequester,
                        IDFriend = friendRequest.IDUser
                    });
                }

                _context.FriendRequests.Remove(friendRequestDB);

                // if both users sent friend requests to eachother,
                // the database will have 2 instances,
                // so you'll have to remove both of them

                var friendRequestDBReverse = await _context.FriendRequests.Where(f => f.IDUser == friendRequest.IDRequester && f.IDRequester == friendRequest.IDUser).FirstOrDefaultAsync();
                if (friendRequestDBReverse != null)
                {
                    _context.FriendRequests.Remove(friendRequestDBReverse);
                }
            }
            else
                throw new InvalidFriendRequestException();
        }
        
        public async Task<bool> DeleteFriendAsync(Friends friend)
        {
            var friend1 = await _context.Friends
                .Where(f => f.IDUser == friend.IDUser && f.IDFriend == friend.IDFriend)
                .FirstOrDefaultAsync();

            var friend2 = await _context.Friends
                .Where(f => f.IDUser == friend.IDFriend && f.IDFriend == friend.IDUser)
                .FirstOrDefaultAsync();

            if (friend1 != null && friend2 != null)
            {
                _context.Friends.Remove(friend1);
                _context.Friends.Remove(friend2);

                return true;
            }
            return false;
        }

        public async Task<List<User>> GetAllFriendsOfUserAsync(int idUser)
        {
            var friendsIds = await _context.Friends.Where(f => f.IDUser == idUser).ToListAsync();

            var friends = new List<User>();

            foreach (var friendId in friendsIds)
            {
                friends.Add(await _context.Users.SingleOrDefaultAsync(u => u.ID == friendId.IDFriend));
            }

            if (friends == null)
            {
                return null;
            }
            return friends;
        }

        public async Task<List<User>> GetAllFriendRequestsOfUserAsync(int idUser)
        {
            var friendRequestsIds = await _context.FriendRequests.Where(f => f.IDUser == idUser).ToListAsync();

            var friendRequests = new List<User>();

            foreach (var friendRequestId in friendRequestsIds)
            {
                friendRequests.Add(await _context.Users.SingleOrDefaultAsync(u => u.ID == friendRequestId.IDRequester));
            }

            if (friendRequests == null)
            {
                return null;
            }
            return friendRequests;
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            var users = await _context.Users.ToListAsync();

            if (users == null)
            {
                return null;
            }
            return users;
        }

        public async Task<Friends> GetFriendOfUserAsync(int idUser, int idFriend)
        {
            var friend = await _context.Friends.Where(u => u.IDUser == idUser && u.IDFriend == idFriend).FirstOrDefaultAsync();

            if (friend == null)
            {
                return null;
            }

            return friend;
        }

        public async Task<FriendRequests> GetFriendRequestOfUserAsync(int idUser, int idRequester)
        {
            var friendRequest = await _context.FriendRequests.Where(u => u.IDUser == idUser && u.IDRequester == idRequester).FirstOrDefaultAsync();

            if (friendRequest == null)
            {
                return null;
            }

            return friendRequest;
        }

        public async Task<FriendRequests> GetFriendRequestByIdAsync(int id)
        {
            var friendRequest = await _context.FriendRequests.Where(u => u.ID == id).FirstOrDefaultAsync();

            if (friendRequest == null)
            {
                return null;
            }

            return friendRequest;
        }
        public async Task<User> GetUserByAccountAsync(string username, string password)
        {
            var user = await _context.Users.Where(u => u.Username == username && u.Password == password).FirstOrDefaultAsync();

            if (user == null)
            {
                return null;
            }
            return user;
        }

        public async Task<User> GetUserByIdAsync(int idUser)
        {
            var user = await _context.Users.Where(u => u.ID == idUser).FirstOrDefaultAsync();

            if (user == null)
            {
                return null;
            }
            return user;
        }
    }
}
