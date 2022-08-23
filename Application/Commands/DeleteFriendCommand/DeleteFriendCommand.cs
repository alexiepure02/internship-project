using Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.DeleteFriendCommand
{
    public class DeleteFriendCommand : IRequest<Friends>
    {
        public int IDUser { get; set; }
        public int IDFriend { get; set; }
    }
}
