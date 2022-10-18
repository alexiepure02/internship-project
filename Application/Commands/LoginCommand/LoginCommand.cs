using MediatR;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.LoginCommand
{
    public class LoginCommand: IRequest<JwtSecurityToken>
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
