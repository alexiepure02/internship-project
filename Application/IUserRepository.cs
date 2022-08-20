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
        void AddUsers(List<User> users);
        void AddUser(User user);
        void RemoveUser(User user);
        // this is find user function
        List<string> GetAllDisplayNames();
        List<int> GetAllIds();
        Friends GetFriendOfUser(User user, User friend);
        User GetUserByUsernameAndPassword(string username, string password);
        User GetUserById(int id);
        void UpdateFriendRequest(User loggedUser, User Friend, bool accepted);
        void ValidateIdFriend(User loggedUser, int idFriend);
        bool CheckIfFriendRequestExists(User loggedUser, User friend);
        void SendFriendRequest(User loggedUser, int idFriend);
        void RemoveFriend(User loggedUser, User Friend);
        int GetUsersCount();
        List<User> GetUsers();
    }
}
