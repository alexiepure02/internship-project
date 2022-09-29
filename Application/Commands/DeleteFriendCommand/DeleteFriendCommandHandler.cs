using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.DeleteFriendCommand
{
    public class DeleteFriendCommandHandler : IRequestHandler<DeleteFriendCommand, Friends>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteFriendCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Friends> Handle(DeleteFriendCommand info, CancellationToken cancellationToken)
        {
            var friend = new Friends
            {
                IDUser = info.IDUser,
                IDFriend = info.IDFriend
            };

            bool completed = await _unitOfWork.UserRepository.DeleteFriendAsync(friend);
            await _unitOfWork.Save();

            if (completed == false)
                return null;

            return friend;
        }
    }
}
