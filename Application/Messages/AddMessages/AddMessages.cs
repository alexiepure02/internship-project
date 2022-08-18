using Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Messages.AddMessages
{
    public class AddMessages : IRequest
    {
        public List<Message> Messages { get; set; }
    }
}
