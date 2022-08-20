using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Users.CheckIfFriendRequestExists
{
    public class CheckIfFriendRequestExistsHandler : IRequestHandler<CheckIfFriendRequestExists, bool>
    {
        private IUserRepository _userRepository;

        public CheckIfFriendRequestExistsHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public Task<bool> Handle(CheckIfFriendRequestExists users, CancellationToken cancellationToken)
        {
            var exists = _userRepository.CheckIfFriendRequestExists(users.LoggedUser, users.Friend);

            return Task.FromResult(exists);
        }
    }
}
