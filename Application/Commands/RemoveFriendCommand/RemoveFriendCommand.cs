using Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.RemoveFriendCommand
{
    public class RemoveFriendCommand : IRequest
    {
        public int IDUser { get; set; }
        public int IDFriend { get; set; }
    }
}
