using Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Messages.AddMessage
{
    public class AddMessage : IRequest
    {
        public int IdSender { get; set; }
        public User Sender { get; set; }
        public int IdReceiver{ get; set; }
        public User Receiver { get; set; }
        public string Message { get; set; }
    }
}
