using Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetAllFriendRequestsOfUserQuery
{
    public class GetAllFriendRequestsOfUserQuery : IRequest<List<User>>
    {
        public int IDUser { get; set; }
    }
}
