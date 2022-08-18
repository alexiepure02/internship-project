using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Messages.CheckProfanity
{
    public class CheckProfanity : IRequest<bool>
    {
        public string Message { get; set; }
    }
}
