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

        public User GetUserByUsernameAndPassword(string username, string password)
        {
            User loggedUser = _users.Find((user) => username == user.Username && password == user.Password);

            return loggedUser;
        }

        public User GetUserById(int id)
        {
            return _users.Find(user => user.ID == id);
        }

        public void UpdateFriendRequest(User loggedUser, User Friend, bool accepted)
        {
            if (accepted == true)
            {
                loggedUser.Friends.Add(Friend);
                _users.Find(user => user.ID == Friend.ID).Friends.Add(loggedUser);
            }
            loggedUser.FriendRequests.Remove(Friend);
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
            if (loggedUser.Friends.Contains(GetUserById(idFriend)))     // workaround?
            {
                throw new UserInFriendsException(idFriend);
            }
        }

        public bool CheckIfFriendRequestExists(User loggedUser, User futureFriend)
        {
            return futureFriend.FriendRequests.Contains(loggedUser);
        }

        public void SendFriendRequest(User loggedUser, int idFutureFriend)
        {
            User futureFriend = GetUserById(idFutureFriend);

            if (futureFriend == null) throw new UserNotFoundException(idFutureFriend);

            if (!CheckIfFriendRequestExists(loggedUser, futureFriend))
                futureFriend.FriendRequests.Add(loggedUser);

        }

        public void RemoveFriend(User loggedUser, User Friend)
        {
            loggedUser.Friends.Remove(Friend);
            _users.Find(user => user.ID == Friend.ID).Friends.Remove(loggedUser);
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
