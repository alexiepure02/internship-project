using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.RegisterCommand
{
    public class RegisterCommand: IRequest<string>
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string DisplayName { get; set; }

    }
}
