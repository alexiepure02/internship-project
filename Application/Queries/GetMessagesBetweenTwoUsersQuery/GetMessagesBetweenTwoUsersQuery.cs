using Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetMessagesBetweenTwoUsersQuery
{
    public class GetMessagesBetweenTwoUsersQuery : IRequest<List<Message>>
    {
        public int idUser1 { get; set; }
        public int idUser2 { get; set; }
    }
}
