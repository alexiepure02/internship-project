using Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Users.GetFriendRequestOfUser
{
    public class GetFriendRequestOfUser : IRequest<FriendRequests>
    {
        public User User { get; set; }
        public User Friend { get; set; }
    }
}
