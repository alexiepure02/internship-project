using Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetFriendRequestByIdQuery
{
    public class GetFriendRequestByIdQuery : IRequest<FriendRequests>
    {
        public int ID { get; set; }
    }
}
