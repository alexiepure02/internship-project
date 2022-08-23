using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.CheckIfFriendExistsQuery
{
    public class CheckIfFriendExistsQueryHandler : IRequestHandler<CheckIfFriendExistsQuery, bool>
    {
        private IUnitOfWork _unitOfWork;

        public CheckIfFriendExistsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(CheckIfFriendExistsQuery info, CancellationToken cancellationToken)
        {
            return await _unitOfWork.UserRepository.CheckIfFriendExistsAsync(info.IDUser, info.IDFriend);
        }
    }
}
