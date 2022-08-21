using Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Users.GetFriendRequestOfUser
{
    public class GetFriendRequestOfUserHandler : IRequestHandler<GetFriendRequestOfUser, FriendRequests>
    {
        private IUserRepository _userRepository;

        public GetFriendRequestOfUserHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public Task<FriendRequests> Handle(GetFriendRequestOfUser info, CancellationToken cancellationToken)
        {
            FriendRequests friendRequests = _userRepository.GetFriendRequestOfUser(info.User, info.Friend);

            return Task.FromResult(friendRequests);
        }
    }
}
