using Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.AddMessageCommand
{
    public class AddMessageCommand : IRequest
    {
        public int IDSender { get; set; }
        public int IDReceiver { get; set; }
        public string Text { get; set; }
    }
}
