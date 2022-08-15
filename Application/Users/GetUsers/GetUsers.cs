using Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Users.GetUsers
{
    public class GetUsers : IRequest<List<User>>
    {
    }
}
