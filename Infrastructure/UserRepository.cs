using Application;
using Domain;
using Domain.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.ClearScript.JavaScript;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole<int>> _roleManager;
        private readonly IConfiguration _configuration;

        public UserRepository(AppDbContext context, UserManager<User> userManager, RoleManager<IdentityRole<int>> roleManager, IConfiguration configuration)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        public async Task<JwtSecurityToken> LoginAsync(string userName, string password)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user != null && await _userManager.CheckPasswordAsync(user, password))
            {
                var userRoles = await _userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
                {
                    new Claim("id", user.Id.ToString()),
                    new Claim("name", user.DisplayName)
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this._configuration.GetConnectionString("SigningKey")));

                var x = this._configuration.GetConnectionString("SigningKey");

                var token = new JwtSecurityToken(
                    issuer: "https://localhost:7228",
                    //audience: "audience",
                    claims: authClaims,
                    expires: DateTime.Now.AddHours(3),
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                    );

                return token;
            }
            return null;
        }

        public async Task<string> RegisterAsync(string userName, string password, string displayName)
        {
            var userExists = await _userManager.FindByNameAsync(userName);

            if (userExists != null)
                return "User already exists.";

            User user = new()
            {
                UserName = userName,
                DisplayName = displayName
            };

            var result = await _userManager.CreateAsync(user, password);

            if (!result.Succeeded)
            {
                return "Failed to create user.";
            }

            return "User created succesfully.";
        }

        public async Task<string> AddToRoleAsync(string userName, string roleName)
        {
            var userExists = await _userManager.FindByNameAsync(userName);

            if (userExists == null)
            {
                return "User does not exist.";
            }

            var role = await _roleManager.FindByNameAsync(roleName);

            if (role == null)
            {
                var roleAdded = await _roleManager.CreateAsync(new IdentityRole<int>
                {
                    Name = roleName
                });
            }

            var adddRoleToUser = await _userManager.AddToRoleAsync(userExists, roleName);

            if (!adddRoleToUser.Succeeded)
            {
                return "Failed to add user to role.";
            }

            return $"User added succesfully to {roleName} role.";
        }

        public async Task CreateFriendRequestAsync(FriendRequests friendRequest)
        {
            var friendRequestDB = await _context.FriendRequests.SingleOrDefaultAsync(f => f.IDUser == friendRequest.IDUser && f.IDRequester == friendRequest.IDRequester);
            var friendDB = await _context.Friends.SingleOrDefaultAsync(f => f.IDUser == friendRequest.IDUser && f.IDFriend == friendRequest.IDRequester);

            if (friendDB != null)
                throw new InvalidFriendRequestException();

            if (friendRequest.IDUser == friendRequest.IDRequester)
                throw new SameIdException(friendRequest.IDUser);
            if (friendRequestDB == null)
                await _context.FriendRequests.AddAsync(friendRequest);
        }

        public async Task UpdateFriendRequestAsync(FriendRequests friendRequest, bool accepted)
        {
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
                friends.Add(await _userManager.FindByIdAsync(friendId.IDFriend.ToString()));
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
                friendRequests.Add(await _userManager.FindByIdAsync(friendRequestId.IDRequester.ToString()));
            }

            if (friendRequests == null)
            {
                return null;
            }
            return friendRequests;
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            var users = await _userManager.Users.ToListAsync();

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

        public async Task<User> GetUserByIdAsync(int idUser)
        {
            var user = await _userManager.FindByIdAsync(idUser.ToString());

            if (user == null)
            {
                return null;
            }
            return user;
        }

        public async Task<User> UpdateDisplayName(int idUser, string newDisplayName)
        {
            var user = await _userManager.FindByIdAsync(idUser.ToString());

            if (user != null)
            {
                user.DisplayName = newDisplayName;

                await _userManager.UpdateAsync(user);

                return user;
            }
            else
                throw new UserNotFoundException(idUser);
        }
    }
}
