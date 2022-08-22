using Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetAllFriendsOfUserQuery
{
    public class GetAllFriendsOfUserQuery : IRequest<List<Friends>>
    {
        public int IDUser { get; set; }
    }
}
