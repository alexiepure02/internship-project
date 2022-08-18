using Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Users.GetUserById
{
    public class GetUserById : IRequest<User>
    {
        public int Id { get; set; }
    }
}
