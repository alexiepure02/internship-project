using Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Users.GetUserByUsernameAndPassword
{
    public class GetUserByUsernameAndPassword : IRequest<User>
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
