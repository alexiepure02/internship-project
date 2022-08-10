using Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Users.SendFriendRequest
{
    public class SendFriendRequest : IRequest
    {
        public User LoggedUser { get; set; }
        public int idFriend { get; set; }
    }
}
