using Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Messages.GetMessagesBetweenTwoUsers
{
    public class GetMessagesBetweenTwoUsers : IRequest<List<Message>>
    {
        public int IdSender { get; set; }
        public int IdReceiver { get; set; }
    }
}
