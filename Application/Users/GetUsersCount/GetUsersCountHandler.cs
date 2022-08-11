using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Users.GetUsersCount
{
    public class GetUsersCountHandler : IRequestHandler<GetUsersCount, int>
    {
        private IUserRepository _userRepository;

        public GetUsersCountHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public Task<int> Handle(GetUsersCount request, CancellationToken cancellationToken)
        {
            var count = _userRepository.GetUsersCount();

            return Task.FromResult(count);
        }
    }
}
