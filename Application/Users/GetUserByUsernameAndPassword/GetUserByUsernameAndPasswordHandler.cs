using Domain;
using MediatR;

namespace Application.Users.GetUserByUsernameAndPassword
{
    public class GetUserByUsernameAndPasswordHandler : IRequestHandler<GetUserByUsernameAndPassword, User>
    {
        private readonly IUserRepository _userRepository;

        public GetUserByUsernameAndPasswordHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public Task<User> Handle(GetUserByUsernameAndPassword userInfo, CancellationToken cancellationToken)
        {
            var user = _userRepository.GetUserByUsernameAndPassword(userInfo.Username, userInfo.Password);

            return Task.FromResult(user);
        }
    }
}
