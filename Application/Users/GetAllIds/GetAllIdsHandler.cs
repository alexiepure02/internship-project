using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Users.GetAllIds
{
    public class GetAllIdsHandler : IRequestHandler<GetAllIds, List<int>>
    {
        private IUserRepository _userRepository;

        public GetAllIdsHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public Task<List<int>> Handle(GetAllIds request, CancellationToken cancellationToken)
        {
            List<int> ids = _userRepository.GetAllIds();

            return Task.FromResult(ids);
        }
    }
}
