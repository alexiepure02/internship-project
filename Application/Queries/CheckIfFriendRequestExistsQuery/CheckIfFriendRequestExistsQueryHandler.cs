using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.CheckIfFriendRequestExistsQuery
{
    public class CheckIfFriendRequestExistsQueryHandler : IRequestHandler<CheckIfFriendRequestExistsQuery, bool>
    {
        private IUnitOfWork _unitOfWork;

        public CheckIfFriendRequestExistsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(CheckIfFriendRequestExistsQuery info, CancellationToken cancellationToken)
        {
            return await _unitOfWork.UserRepository.CheckIfFriendRequestExistsAsync(info.IDUser, info.IDRequester);
        }
    }
}
