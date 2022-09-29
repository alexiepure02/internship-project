using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.AddToRoleCommand
{
    public class AddToRoleCommandHandler : IRequestHandler<AddToRoleCommand, string>
    {
        private readonly IUnitOfWork _unitOfWork;

        public AddToRoleCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<string> Handle(AddToRoleCommand info, CancellationToken cancellationToken)
        {
            return await _unitOfWork.UserRepository.AddToRoleAsync(info.UserName, info.RoleName);
        }
    }
}
