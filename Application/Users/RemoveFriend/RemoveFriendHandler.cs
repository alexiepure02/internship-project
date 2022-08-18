using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Users.RemoveFriend
{
    public class RemoveFriendHandler : IRequestHandler<RemoveFriend>
    {
        private IUserRepository _userRepository;

        public RemoveFriendHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public Task<Unit> Handle(RemoveFriend users, CancellationToken cancellationToken)
        {
            _userRepository.RemoveFriend(users.LoggedUser, users.IdFriend);

            return Task.FromResult(Unit.Value);
        }
    }
}
