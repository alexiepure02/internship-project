using Application;
using Domain;
using Domain.Exceptions;

namespace Infrastructure
{
    public class InMemoryUserRepository : IUserRepository
    {
        private readonly List<User> _users = new();

        public void AddUsers(List<User> users)
        {
            foreach (User user in users)
            {
                _users.Add(user);
            }
        }

        public void AddUser(User user)
        {
            _users.Add(user);
        }

        public void RemoveUser(User user)
        {
            _users.Remove(user);
        }

        public List<string> GetAllDisplayNames()
        {
            List<string> displayNames = new();

            foreach (User user in _users)
            {
                displayNames.Add(user.DisplayName);
            }

            return displayNames;
        }

        public List<int> GetAllIds()
        {
            List<int> ids = new();

            foreach (User user in _users)
            {
                ids.Add(user.ID);
            }

            return ids;
        }

        public Friends GetFriendOfUser(User user, User friend)
        {
            return user.Friends.Where(f => f.IDUser == user.ID && f.IDFriend == friend.ID).FirstOrDefault();
        }

        public User GetUserByUsernameAndPassword(string username, string password)
        {
            User loggedUser = _users.Find((user) => username == user.Username && password == user.Password);

            return loggedUser;
        }

        public User GetUserById(int id)
        {
            return _users.Find(user => user.ID == id);
        }

        public void UpdateFriendRequest(User loggedUser, User friendUser, bool accepted)
        {
            if (accepted == true)
            {
                // in theory, this works

                Friends friend = new Friends
                {
                    IDUser = loggedUser.ID,
                    User = loggedUser,
                    IDFriend = friendUser.ID,
                    Friend = friendUser
                };

                Friends user = new Friends
                {
                    IDUser = friendUser.ID,
                    User = friendUser,
                    IDFriend = loggedUser.ID,
                    Friend = loggedUser
                };

                loggedUser.Friends.Add(friend);
                _users.Find(user => user.ID == friendUser.ID).Friends.Add(user);
            }

            loggedUser.FriendRequests.Remove(friendUser);
        }

        public void ValidateIdFriend(User loggedUser, int idFriend)
        {
            if (loggedUser.ID == idFriend)
            {
                throw new SameIdException(idFriend);
            }
            if (GetUserById(idFriend) == null)
            {
                throw new UserNotFoundException(idFriend);
            }

            // this won't work because ID field is different
            // .contains can't find the friend

            /*Friends friend = new Friends
            {
                IDUser = loggedUser.ID,
                User = loggedUser,
                IDFriend = idFriend,
                Friend = GetUserById(idFriend)
            }*/;

            Friends friend = GetFriendOfUser(loggedUser, GetUserById(idFriend));

            if (friend != null)
            {
                throw new UserInFriendsException(idFriend);
            }
        }

        public bool CheckIfFriendRequestExists(User user, User friend)
        {
            return user.FriendRequests.Contains(friend);
        }

        public void SendFriendRequest(User loggedUser, int idFutureFriend)
        {
            User futureFriend = GetUserById(idFutureFriend);

            if (futureFriend == null) throw new UserNotFoundException(idFutureFriend);

            if (!CheckIfFriendRequestExists(futureFriend, futureFriend))
            {
                futureFriend.FriendRequests.Add(loggedUser);
            }
        }

        public void RemoveFriend(User loggedUser, User friend)
        {
            Friends friendInLoggedUser = GetFriendOfUser(loggedUser, friend);
            Friends loggedUserInFriend = GetFriendOfUser(friend, loggedUser);

            loggedUser.Friends.Remove(friendInLoggedUser);
            _users.Find(user => user.ID == friend.ID).Friends.Remove(loggedUserInFriend);
        }

        public int GetUsersCount()
        {
            return _users.Count;
        }

        public List<User> GetUsers()
        {
            return _users;
        }
    }
}
