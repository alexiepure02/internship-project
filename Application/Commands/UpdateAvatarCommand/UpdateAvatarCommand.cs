using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.UpdateAvatarCommand
{
    public class UpdateAvatarCommand : IRequest<string>
    {
        public int IdUser { get; set; }
        public string ImagePath { get; set; }
    }
}
