using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetAvatarByIdQuery
{
    public class GetAvatarByIdQuery : IRequest<string>
    {
        public int IdUser { get; set; }
    }
}
