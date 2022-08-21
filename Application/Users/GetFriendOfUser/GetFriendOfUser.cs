using Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Users.GetFriendOfUser
{
    public class GetFriendOfUser : IRequest<Friends>
    {
        public User User { get; set; }
        public User Friend { get; set; }
    }
}
