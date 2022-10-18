using Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetFriendRequestByIdQuery
{
    public class GetFriendRequestByIdQueryHandler : IRequestHandler<GetFriendRequestByIdQuery, FriendRequests>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetFriendRequestByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<FriendRequests> Handle(GetFriendRequestByIdQuery info, CancellationToken cancellationToken)
        {
            return await _unitOfWork.UserRepository.GetFriendRequestByIdAsync(info.ID);
        }
    }
}
