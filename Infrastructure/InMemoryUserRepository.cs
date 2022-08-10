using Application;
using ConsoleChatApp;
using ConsoleChatApp.Domain.Exceptions;
using Domain;

namespace Infrastructure
{
    public class InMemoryUserRepository : IUserRepository
    {
        private readonly List<User> _users = new();

        public InMemoryUserRepository(List<User> users)
        {
            _users = users;
        }

        public User GetUserByUsernameAndPassword(string username, string password)
        {
            User loggedUser = _users.Find((user) => username == user.Username && password == user.Password);

            return loggedUser == null ? throw new UserNotFoundException(username) : loggedUser;
        }

        public User GetUserById(int id)
        {
            return _users.Find(user => user.Id == id);
        }

        public void AcceptOrRemoveFriendRequest(User loggedUser, int idFriend, bool removeFriendRequest)
        {
            if (removeFriendRequest == false)
            {
                loggedUser.Friends.Add(idFriend);
                _users.Find(user => user.Id == idFriend).Friends.Add(loggedUser.Id);
            }
            loggedUser.FriendRequests.Remove(idFriend);
        }

        public void ValidateIdFriend(User loggedUser, int idFriend)
        {
            if (loggedUser.Id == idFriend)
            {
                throw new SameIdException(idFriend);
            }
            if (GetUserById(idFriend) == null)
            {
                throw new UserNotFoundException(idFriend);
            }
            if (loggedUser.Friends.Contains(idFriend))
            {
                // for some reason, it doesn't see the exception unless I do using ConsoleChatApp;
                throw new UserInFriendsException(idFriend);
            }
        }

        public bool CheckIfFriendRequestExists(User loggedUser, User futureFriend)
        {
            return futureFriend.FriendRequests.Contains(loggedUser.Id);
        }

        public void SendFriendRequest(User loggedUser, int idFutureFriend)
        {
            User futureFriend = _users.Find(user => user.Id == idFutureFriend);

            if (!CheckIfFriendRequestExists(loggedUser, futureFriend))
                futureFriend.FriendRequests.Add(loggedUser.Id);

        }

        public void DeleteFriend(User loggedUser, int idFriend)
        {
            loggedUser.Friends.Remove(idFriend);
            _users.Find(user => user.Id == idFriend).Friends.Remove(loggedUser.Id);
        }
    }
}
