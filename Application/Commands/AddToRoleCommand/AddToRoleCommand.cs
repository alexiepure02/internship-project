using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.AddToRoleCommand
{
    public class AddToRoleCommand: IRequest<string>
    {
        public string UserName { get; set; }
        public string RoleName { get; set; }
    }
}
