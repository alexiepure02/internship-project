using ConsoleChatApp.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Users.GetUserByUsernameAndPassword
{
    public class GetUserByUsernameAndPasswordHandler : IRequestHandler<GetUserByUsernameAndPassword, User>
    {
        private readonly IUserRepository _userRepository;

        public GetUserByUsernameAndPasswordHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public Task<User> Handle(GetUserByUsernameAndPassword request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
