using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.UpdateFriendRequestCommand
{
    public class UpdateFriendRequestCommandHandler : IRequestHandler<UpdateFriendRequestCommand, FriendRequests>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateFriendRequestCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<FriendRequests> Handle(UpdateFriendRequestCommand info, CancellationToken cancellationToken)
        {
            var friendRequest = new FriendRequests
            {
                IDUser = info.IDUser,
                IDRequester = info.IDRequester
            };

            await _unitOfWork.UserRepository.UpdateFriendRequestAsync(friendRequest, info.Accepted);
            await _unitOfWork.Save();

            return friendRequest;
        }
    }
}
