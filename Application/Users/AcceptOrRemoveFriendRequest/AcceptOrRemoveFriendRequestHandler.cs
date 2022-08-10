using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Users.AcceptOrRemoveFriendRequest
{
    public class AcceptOrRemoveFriendRequestHandler : IRequestHandler<AcceptOrRemoveFriendRequest>
    {
        private IUserRepository _userRepository;

        public AcceptOrRemoveFriendRequestHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public Task<Unit> Handle(AcceptOrRemoveFriendRequest info, CancellationToken cancellationToken)
        {
            _userRepository.AcceptOrRemoveFriendRequest(info.LoggedUser, info.idFriend, info.removeFriendRequest);

            return Task.FromResult(Unit.Value);
        }
    }
}
