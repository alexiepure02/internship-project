using Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.CreateFriendRequestCommand
{
    public class CreateFriendRequestCommandHandler : IRequestHandler<CreateFriendRequestCommand, FriendRequests>
    {
        private IUnitOfWork _unitOfWork;

        public CreateFriendRequestCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<FriendRequests> Handle(CreateFriendRequestCommand info, CancellationToken cancellationToken)
        {
            var friendRequest = new FriendRequests
            {
                IDUser = info.IDUser,
                IDRequester = info.IDRequester
            };

            await _unitOfWork.UserRepository.CreateFriendRequestAsync(friendRequest);
            await _unitOfWork.Save();

            return friendRequest;
        }
    }
}
