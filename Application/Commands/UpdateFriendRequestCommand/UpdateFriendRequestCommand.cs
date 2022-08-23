using Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.UpdateFriendRequestCommand
{
    public class UpdateFriendRequestCommand : IRequest<FriendRequests>
    {
        public int IDUser { get; set; }
        public int IDRequester { get; set; }
        public bool Accepted { get; set; }
    }
}
