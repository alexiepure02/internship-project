using Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetUserByAccountQuery
{
    public class GetUserByAccountQuery : IRequest<User>
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
