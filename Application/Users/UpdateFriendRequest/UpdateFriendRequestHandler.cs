using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Users.UpdateFriendRequest
{
    public class UpdateFriendRequestHandler : IRequestHandler<UpdateFriendRequest>
    {
        private IUserRepository _userRepository;

        public UpdateFriendRequestHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public Task<Unit> Handle(UpdateFriendRequest info, CancellationToken cancellationToken)
        {
            _userRepository.UpdateFriendRequest(info.LoggedUser, info.Friend, info.Accepted);

            return Task.FromResult(Unit.Value);
        }
    }
}
