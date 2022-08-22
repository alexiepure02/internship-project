using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.CheckIfFriendExistsQuery
{
    public class CheckIfFriendExistsQuery : IRequest<bool>
    {
        public int IDUser { get; set; }
        public int IDFriend { get; set; }
    }
}
