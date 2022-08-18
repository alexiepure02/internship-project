using Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Users.AddUsers
{
    public class AddUsers : IRequest
    {
        public List<User> Users { get; set; }
    }
}
