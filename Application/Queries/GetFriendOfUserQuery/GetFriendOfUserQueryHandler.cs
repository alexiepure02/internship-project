using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetFriendOfUserQuery
{
    public class GetFriendOfUserQueryHandler : IRequestHandler<GetFriendOfUserQuery, Friends>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetFriendOfUserQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Friends> Handle(GetFriendOfUserQuery info, CancellationToken cancellationToken)
        {
            return await _unitOfWork.UserRepository.GetFriendOfUserAsync(info.IDUser, info.IDFriend);
        }
    }
}
