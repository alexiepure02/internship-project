using Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Users.GetFriendOfUser
{
    public class GetFriendOfUserHandler : IRequestHandler<GetFriendOfUser, Friends>
    {
        private IUserRepository _userRepository;

        public GetFriendOfUserHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public Task<Friends> Handle(GetFriendOfUser info, CancellationToken cancellationToken)
        {
            Friends friend = _userRepository.GetFriendOfUser(info.User, info.Friend);

            return Task.FromResult(friend);
        }
    }
}
