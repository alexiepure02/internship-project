using Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.CreateUserCommand
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, User>
    {
        private IUnitOfWork _unitOfWork;

        public CreateUserCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<User> Handle(CreateUserCommand info, CancellationToken cancellationToken)
        {
            var user = new User
            {
                Username = info.Username,
                Password = info.Password,
                DisplayName = info.DisplayName
            };
            await _unitOfWork.UserRepository.CreateUserAsync(user);
            await _unitOfWork.Save();

            return user;
        }
    }
}
