using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Users.ValidateIdFriend
{
    public class SendFriendRequestHandler : IRequestHandler<ValidateIdFriend>
    {
        private IUserRepository _userRepository;

        public SendFriendRequestHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public Task<Unit> Handle(ValidateIdFriend users, CancellationToken cancellationToken)
        {
            _userRepository.ValidateIdFriend(users.LoggedUser, users.idFriend);

            return Task.FromResult(Unit.Value);
        }
    }
}
