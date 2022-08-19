using Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Users.UpdateFriendRequest
{
    public class UpdateFriendRequest : IRequest
    {
        public User LoggedUser { get; set; }
        public User Friend { get; set; }
        public bool Accepted { get; set; }
    }
}
