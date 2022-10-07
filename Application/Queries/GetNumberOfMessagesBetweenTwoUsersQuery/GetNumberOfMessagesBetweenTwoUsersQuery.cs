using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetNumberOfMessagesBetweenTwoUsersQuery
{
    public class GetNumberOfMessagesBetweenTwoUsersQuery : IRequest<int>
    {
        public int IDUser1 { get; set; }
        public int IDUser2 { get; set; }
    }
}
