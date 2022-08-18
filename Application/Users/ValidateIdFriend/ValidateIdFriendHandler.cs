using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Users.ValidateIdFriend
{
    public class ValidateIdFriendHandler : IRequestHandler<ValidateIdFriend>
    {
        private IUserRepository _userRepository;

        public ValidateIdFriendHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public Task<Unit> Handle(ValidateIdFriend users, CancellationToken cancellationToken)
        {
            try
            {
                _userRepository.ValidateIdFriend(users.LoggedUser, users.IdFriend);
            }
            catch (Exception ex)
            {
                return Task.FromException<Unit>(ex);
            }

            return Task.FromResult(Unit.Value);
        }
    }
}
