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
        // this is find user function
        User GetUserByUsernameAndPassword(string username, string password);
        User GetUserById(int id);
        void AcceptOrRemoveFriendRequest(User loggedUser, int idFriend, bool removeFriendRequest);
        void ValidateIdFriend(User loggedUser, int idFriend);
        bool CheckIfFriendRequestExists(User loggedUser, User futureFriend);
        void SendFriendRequest(User loggedUser, int idFriend);
        void DeleteFriend(User loggedUser, int idFriend);
    }
}
