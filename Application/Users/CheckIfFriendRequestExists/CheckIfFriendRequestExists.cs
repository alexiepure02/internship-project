using Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Users.CheckIfFriendRequestExists
{
    public class CheckIfFriendRequestExists : IRequest<bool>
    {
        public User LoggedUser { get; set; }
        public User Friend { get; set; }
    }
}
