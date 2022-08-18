using Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Messages.GetMessages
{
    public class GetMessages : IRequest<List<Message>>
    {
    }
}
