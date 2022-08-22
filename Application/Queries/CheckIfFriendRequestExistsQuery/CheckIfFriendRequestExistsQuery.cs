using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.CheckIfFriendRequestExistsQuery
{
    public class CheckIfFriendRequestExistsQuery : IRequest<bool>
    {
        public int IDUser { get; set; }
        public int IDRequester { get; set; }
    }
}
