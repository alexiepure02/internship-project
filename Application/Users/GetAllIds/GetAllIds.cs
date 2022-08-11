using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Users.GetAllIds
{
    public class GetAllIds : IRequest<List<int>>
    {
    }
}
