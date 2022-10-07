using Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.UpdateUserCommand
{
    public class UpdateUserDisplayNameCommand : IRequest<User>
    {
        public int IdUser { get; set; }
        public string NewDisplayName { get; set; }
    }
}
