using MediatR;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.LoginCommand
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, JwtSecurityToken>
    {
        private readonly IUnitOfWork _unitOfWork;

        public LoginCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<JwtSecurityToken> Handle(LoginCommand info, CancellationToken cancellationToken)
        {
            var token = await _unitOfWork.UserRepository.LoginAsync(info.UserName, info.Password);

            if (token == null)
            {
                return null;
            }

            return token;
        }
    }
}
