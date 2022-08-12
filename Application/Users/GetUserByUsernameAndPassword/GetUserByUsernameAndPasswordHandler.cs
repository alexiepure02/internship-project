using Domain;
using Domain.Exceptions;
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
            try
            {
                var user = _userRepository.GetUserByUsernameAndPassword(userInfo.Username, userInfo.Password);
                return Task.FromResult(user);
            }
            catch (UserNotFoundException ex)
            {
                return Task.FromException<User>(ex);
            }

        }
    }
}
