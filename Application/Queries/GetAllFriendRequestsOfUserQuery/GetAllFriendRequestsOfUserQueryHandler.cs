using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetAllFriendRequestsOfUserQuery
{
    internal class GetAllFriendRequestsOfUserQueryHandler : IRequestHandler<GetAllFriendRequestsOfUserQuery, List<FriendRequests>>
    {
        private IUnitOfWork _unitOfWork;

        public GetAllFriendRequestsOfUserQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<FriendRequests>> Handle(GetAllFriendRequestsOfUserQuery info, CancellationToken cancellationToken)
        {
            return await _unitOfWork.UserRepository.GetAllFriendRequestsOfUserAsync(info.IDUser);
        }
    }
}
