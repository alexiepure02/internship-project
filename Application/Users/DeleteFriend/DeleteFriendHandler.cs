using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Users.DeleteFriend
{
    public class DeleteFriendHandler : IRequestHandler<DeleteFriend>
    {
        private IUserRepository _userRepository;

        public DeleteFriendHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public Task<Unit> Handle(DeleteFriend users, CancellationToken cancellationToken)
        {
            _userRepository.DeleteFriend(users.LoggedUser, users.IdFriend);

            return Task.FromResult(Unit.Value);
        }
    }
}
