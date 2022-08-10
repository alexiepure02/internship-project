using Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Users.DeleteFriend
{
    public class DeleteFriend : IRequest
    {
        public User LoggedUser { get; set; }
        public int IdFriend { get; set; }
    }
}
