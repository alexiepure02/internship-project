using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Users.AddUsers
{
    internal class AddUsersHandler : IRequestHandler<AddUsers>
    {
        private readonly IUserRepository _userRepository;

        public AddUsersHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public Task<Unit> Handle(AddUsers users, CancellationToken cancellationToken)
        {
            _userRepository.AddUsers(users.Users);

            // returns void
            return Task.FromResult(Unit.Value);
        }
    }
}
