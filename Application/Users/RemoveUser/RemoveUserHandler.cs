using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Users.RemoveUser
{
    public class RemoveUserHandler : IRequestHandler<RemoveUser>
    {
        private IUserRepository _userRepository;

        public RemoveUserHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public Task<Unit> Handle(RemoveUser user, CancellationToken cancellationToken)
        {
            _userRepository.RemoveUser(user.User);

            return Task.FromResult(Unit.Value);
        }
    }
}
