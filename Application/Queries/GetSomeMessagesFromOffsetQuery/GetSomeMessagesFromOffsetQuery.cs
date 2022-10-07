using Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetSomeMessagesFromOffsetQuery
{
    public class GetSomeMessagesFromOffsetQuery: IRequest<List<Message>>
    {
        public int IDUser1 { get; set; }
        public int IDUser2 { get; set; }
        public int Offset { get; set; }
        public int NumberOfMessages { get; set; }
    }
}
