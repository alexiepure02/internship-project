using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Users.AddUser
{
    public class AddUserHandler : IRequestHandler<AddUser>
    {
        private IUserRepository _userRepository;

        public AddUserHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public Task<Unit> Handle(AddUser user, CancellationToken cancellationToken)
        {
            _userRepository.AddUser(user.User);

            return Task.FromResult(Unit.Value);
        }
    }
}
