using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Users.SendFriendRequest
{
    public class SendFriendRequestHandler : IRequestHandler<SendFriendRequest>
    {
        private IUserRepository _userRepository;

        public SendFriendRequestHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public Task<Unit> Handle(SendFriendRequest users, CancellationToken cancellationToken)
        {
            try
            {
                _userRepository.SendFriendRequest(users.LoggedUser, users.idFriend);
            }
            catch
            {
                throw;
            }

            return Task.FromResult(Unit.Value);
        }
    }
}
