﻿using Application;
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
                ids.Add(user.Id);
            }

            return ids;
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

            if (futureFriend == null) throw new UserNotFoundException(idFutureFriend);

            if (!CheckIfFriendRequestExists(loggedUser, futureFriend))
                futureFriend.FriendRequests.Add(loggedUser.Id);

        }

        public void RemoveFriend(User loggedUser, int idFriend)
        {
            loggedUser.Friends.Remove(idFriend);
            _users.Find(user => user.Id == idFriend).Friends.Remove(loggedUser.Id);
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
