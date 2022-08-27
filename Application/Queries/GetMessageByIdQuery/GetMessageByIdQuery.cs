using Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetMessageByIdQuery
{
    public class GetMessageByIdQuery : IRequest<Message>
    {
        public int IDMessage { get; set; }
    }
}
