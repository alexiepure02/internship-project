using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Users.GetAllDisplayNames
{
    public class GetAllDisplayNamesHandler : IRequestHandler<GetAllDisplayNames, List<string>>
    {
        private IUserRepository _userRepository;

        public GetAllDisplayNamesHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public Task<List<string>> Handle(GetAllDisplayNames request, CancellationToken cancellationToken)
        {
            List<string> displayNames = _userRepository.GetAllDisplayNames();
            
            return Task.FromResult(displayNames);
        }
    }
}
