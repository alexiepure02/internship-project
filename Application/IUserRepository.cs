using ConsoleChatApp.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public interface IUserRepository
    {
        // this is find user function
        User GetUserByUsernameAndPassword(string username, string password);
        User GetUserById(int id);
        void AcceptFriendRequest(User loggedUser, int idFriend);
        void ValidateFriendId(User loggedUser, int id);
        bool CheckIfFriendRequestExists(User loggedUser, int idFriend);
        void SendFriendRequest(User loggedUser, int idFriend);
        void DeleteFriend(User loggedUser, int idFriend);
    }
}
