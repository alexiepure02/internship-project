using Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Users.GetUsers
{
    internal class GetUsersHandler : IRequestHandler<GetUsers, List<User>>
    {
        private IUserRepository _userRepository;

        public GetUsersHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public Task<List<User>> Handle(GetUsers request, CancellationToken cancellationToken)
        {
            List<User> users = _userRepository.GetUsers();

            return Task.FromResult(users);
        }
    }
}
