using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Users.ValidateNewUser
{
    public class ValidateNewUserHandler : IRequestHandler<ValidateNewUser, string>
    {
        private IUserRepository _userRepository;

        public ValidateNewUserHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public Task<string> Handle(ValidateNewUser info, CancellationToken cancellationToken)
        {
            string result = "";
            result = _userRepository.ValidateNewUser(info.User);

            return Task.FromResult(result);
        }
    }
}
