using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetFriendRequestOfUserQuery
{
    public class GetFriendRequestOfUserQueryHandler : IRequestHandler<GetFriendRequestOfUserQuery, FriendRequests>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetFriendRequestOfUserQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<FriendRequests> Handle(GetFriendRequestOfUserQuery info, CancellationToken cancellationToken)
        {
            return await _unitOfWork.UserRepository.GetFriendRequestOfUserAsync(info.IDUser, info.IDRequester);
        }
    }
}
